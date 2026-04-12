using ExaminationSystem.Features.Account.ForgetResetPassword.DTOs;
using ExaminationSystem.Features.Account.Shared.Services;
using static ExaminationSystem.Features.Account.ForgetResetPassword.Helper.HashToken;

namespace ExaminationSystem.Features.Account.ForgetResetPassword.Forgot_ResetPassword
{
    public record ForgotPasswordCommand(string Email) : IRequest<RequestResult<ForgotPasswordResponse>>;


    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, RequestResult<ForgotPasswordResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public ForgotPasswordHandler(UserManager<User> userManager,
            IUnitOfWork unitOfWork,
            IEmailService emailService
            )
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _emailService = emailService;

        }

        public async Task<RequestResult<ForgotPasswordResponse>> Handle(
            ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            // 2. التحقق من Lockout لـ ForgotPassword = المستخدم في حاله الحظر 
            if (user != null && user.ForgotPasswordLockoutEnd > DateTime.UtcNow)
            {
                // لا نكشف السبب الحقيقي - نفس الرد العام
                return RequestResult<ForgotPasswordResponse>.Success(new ForgotPasswordResponse
                {
                    Message = "If your email is registered, you will receive a password reset link.",
                    EmailSent = false
                });
            }

            // 3. زيادة عداد المحاولات الخاطئة إذا كان المستخدم موجود// مش في فترة الحظر
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

            // 4. إذا المستخدم غير موجود -> نفس الرد العام
            if (user == null)
            {
                return RequestResult<ForgotPasswordResponse>.Success(new ForgotPasswordResponse
                {
                    Message = "If your email is registered, you will receive a password reset link.",
                    EmailSent = false
                });
            }

            // 5. إعادة تعيين العداد بعد نجاح الطلب
            user.ForgotPasswordAttempts = 0;
            user.ForgotPasswordLockoutEnd = null;
            await _userManager.UpdateAsync(user);


            // 6. إنشاء التوكن (UUID - مرة واحدة)
            var resetToken = Guid.NewGuid().ToString();

            // 7. تخزين التوكن كـ Hash في قاعدة البيانات
            var tokenHash = TokenHasher.HashToken(resetToken);


            var passwordResetToken = new PasswordResetToken
            {
                UserId = user.Id,
                TokenHash = resetToken,
                ExpiryAt = DateTime.UtcNow.AddMinutes(15),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.GetRepository<PasswordResetToken>().Add(passwordResetToken);
            await _unitOfWork.SaveChangesAsync();

            // 8. إرسال الإيميل
            var emailSent = await _emailService.SendPasswordResetEmailAsync(user.Email, resetToken, user.UserName);

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