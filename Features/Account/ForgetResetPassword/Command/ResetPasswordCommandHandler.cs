using ExaminationSystem.Features.Account.ForgetResetPassword.DTOs;
using ExaminationSystem.Features.Common.Request;
using static ExaminationSystem.Features.Account.ForgetResetPassword.Helper.HashToken;

namespace ExaminationSystem.Features.Account.ForgetResetPassword.Forgot_ResetPassword
{

    public record ResetPasswordCommand(string Email, string Token, string NewPassword, string ConfirmNewPassword)
        : IRequest<RequestResult<ResetPasswordResponse>>;

    public class ResetPasswordHandler 
        : IRequestHandler<ResetPasswordCommand, RequestResult<ResetPasswordResponse>>
    {

        private readonly UserManager<User> _userManager;
        private readonly IGeneralRepository<PasswordResetToken> _repo;

        public ResetPasswordHandler(UserManager<User> userManager, IGeneralRepository<PasswordResetToken> repo)
        {
            _userManager = userManager;
            _repo = repo;
        }

        public async Task<RequestResult<ResetPasswordResponse>> Handle(
            ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // VALIDATION LAYER 1: Password confirmation check
            // Purpose: Ensure user didn't mistype new password
            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return RequestResult<ResetPasswordResponse>.Failure
                    ("Passwords do not match", RequestErrorCode.PasswordResetFailed);
            }

            // SECURITY LAYER 1: Hash the received token
            // Purpose: Compare hash-with-hash (never compare plain tokens)
            // The token from user email is plain-text, we hash it to match DB
            var tokenHash = TokenHasher.HashToken(request.Token);

            // SECURITY LAYER 2: Validate token against database
            // 1. Token hash matches (integrity)
            // 2. Token not used before (single-use)
            // 3. Token not expired (15 min validity)
            var tokenEntity = await _repo
                .Get(t => t.TokenHash == tokenHash 
                && !t.IsUsed
                && t.ExpiryAt > DateTime.UtcNow 
                && t.User.Email == request.Email)
                .Include(t => t.User)
                .FirstOrDefaultAsync(cancellationToken);

            // SECURITY LAYER 3: Reject invalid/expired/used tokens
            // Purpose: Prevent replay attacks and expired link usage
            if (tokenEntity == null)
            {
                return RequestResult<ResetPasswordResponse>.Failure
                    ("Invalid or expired token", RequestErrorCode.InvalidToken);
            }

            if (tokenEntity.ExpiryAt < DateTime.UtcNow)
            {
                return RequestResult<ResetPasswordResponse>.Failure
                    ("Invalid or expired token", RequestErrorCode.InvalidToken);
            }

            var user = tokenEntity.User;

            // PASSWORD UPDATE: Remove existing password
            // Purpose: Clear old password hash before setting new one
            // Required by Identity: Cannot add new password if one exists
            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                return RequestResult<ResetPasswordResponse>.Failure
                    ("Failed to reset password", RequestErrorCode.PasswordResetFailed);
            }

            // PASSWORD UPDATE: Set new password
            // Password will be automatically hashed by UserManager
            // Bcrypt with salt rounds >= 12 applied automatically
            var addPasswordResult = await _userManager.AddPasswordAsync(user, request.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                return RequestResult<ResetPasswordResponse>.Failure
                    ("Failed to set new password", RequestErrorCode.PasswordResetFailed);
            }

            // TOKEN INVALIDATION: Mark token as used
            // Purpose: Prevent reuse of same token (single-use policy)
            tokenEntity.IsUsed = true;
            tokenEntity.UpdatedAt = DateTime.UtcNow;

            // SECURITY LAYER 4: Invalidate all existing sessions
            // Purpose: Force logout from all devices after password change
            // SecurityStamp change invalidates all old JWT tokens
            await _userManager.UpdateSecurityStampAsync(user);

            await _repo.SaveChangesAsync();

            return RequestResult<ResetPasswordResponse>.Success(new ResetPasswordResponse
            {
                Success = true,
                Message = "Password has been reset successfully. Please login with your new password."
            });
        }


    }
}