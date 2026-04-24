using ExaminationSystem.Features.Account.ForgetResetPassword.DTOs;
using ExaminationSystem.Features.Account.Shared.Services;
using ExaminationSystem.Features.Common.Request;
using static ExaminationSystem.Features.Account.ForgetResetPassword.Helper.HashToken;

namespace ExaminationSystem.Features.Account.ForgetResetPassword.Forgot_ResetPassword
{
    public record ForgotPasswordCommand(string Email) : IRequest<RequestResult<ForgotPasswordResponse>>;


    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, RequestResult<ForgotPasswordResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IGeneralRepository<PasswordResetToken> _repository;

        public ForgotPasswordHandler(UserManager<User> userManager,
            IEmailService emailService,
            IGeneralRepository<PasswordResetToken> repository)
        {
            _userManager = userManager;
            _emailService = emailService;
            _repository = repository;
        }

        public async Task<RequestResult<ForgotPasswordResponse>> Handle(
            ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            // SECURITY LAYER 1: Check if user is in lockout period
            // Purpose: Prevent brute force attacks on forgot password endpoint
            // Returns same message as "user not found" to avoid email enumeration
            if (user != null && user.ForgotPasswordLockoutEnd > DateTime.UtcNow)
            {
                return RequestResult<ForgotPasswordResponse>.Success(new ForgotPasswordResponse
                {
                    Message = "If your email is registered, you will receive a password reset link.",
                    EmailSent = false
                });
            }

            // SECURITY LAYER 2: Track failed attempts
            // Purpose: Increment counter for each reset request
            // After 5 attempts -> 15 minute lockout
            if (user != null)
            {
                user.ForgotPasswordAttempts++;

                if (user.ForgotPasswordAttempts >= 5)
                {
                    user.ForgotPasswordLockoutEnd = DateTime.UtcNow.AddMinutes(15);
                    await _userManager.UpdateAsync(user);

                    return RequestResult<ForgotPasswordResponse>.Success(new ForgotPasswordResponse
                    {
                        Message = "Too many attempts. Please try again after 15 minutes.",
                        EmailSent = false
                    });
                }

                await _userManager.UpdateAsync(user);
            }

            // SECURITY LAYER 3: Generic response for non-existent emails
            // Purpose: Prevent email enumeration attacks
            // Always return same message whether email exists or not
            if (user == null)
            {
                return RequestResult<ForgotPasswordResponse>.Success(new ForgotPasswordResponse
                {
                    Message = "If your email is registered, you will receive a password reset link.",
                    EmailSent = false
                });
            }

            // Reset attempt counter on successful request
            // Purpose: Allow legitimate user to continue after valid request
            user.ForgotPasswordAttempts = 0;
            user.ForgotPasswordLockoutEnd = null;
            await _userManager.UpdateAsync(user);


            // Generate secure reset token
            // Use GUID for uniqueness and randomness
            var resetToken = Guid.NewGuid().ToString();


            // CRITICAL SECURITY: Hash the token before storing
            // Purpose: Prevent token theft from database compromise
            // Never store plain-text tokens in database
            var tokenHash = TokenHasher.HashToken(resetToken);


            var passwordResetToken = new PasswordResetToken
            {
                UserId = user.Id,
                TokenHash = tokenHash,
                ExpiryAt = DateTime.UtcNow.AddMinutes(15),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            _repository.Add(passwordResetToken);
            await _repository.SaveChangesAsync();

            // Send email with plain-text token (not hash)
            // User receives the plain token to click in email link
            // Database stores only the hash for verification
            var emailSent = await _emailService.SendPasswordResetEmailAsync
                (user.Email, resetToken, user.UserName);

            if (!emailSent)
            {
                return RequestResult<ForgotPasswordResponse>.Failure("", RequestErrorCode.EmailSendFailed);
            }

            return RequestResult<ForgotPasswordResponse>.Success(new ForgotPasswordResponse
            {
                Message = "Password reset link has been sent to your email.",
                EmailSent = true
            });
        }

    }
}