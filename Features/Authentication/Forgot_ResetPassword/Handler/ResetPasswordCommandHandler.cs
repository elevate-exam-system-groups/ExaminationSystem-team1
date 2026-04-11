namespace ExaminationSystem.Features.Authentication.Forgot_ResetPassword.Handler
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, RequestResult<ResetPasswordResponse>>
    {

        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ResetPasswordHandler(UserManager<User> userManager,IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResult<ResetPasswordResponse>> Handle(
            ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return RequestResult<ResetPasswordResponse>.Failure(ErrorCode.PasswordResetFailed);
            }

            // 3. هاش التوكن المُستلم
            var tokenHash = TokenHasher.HashToken(request.Token);

            // 4. البحث عن التوكن في قاعدة البيانات
            var tokenEntity = await _unitOfWork.GetRepository<PasswordResetToken>()
                .Get(t => t.TokenHash == tokenHash && !t.IsUsed)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TokenHash == tokenHash && !t.IsUsed, cancellationToken);

            if (tokenEntity == null)
            {
                return RequestResult<ResetPasswordResponse>.Failure(ErrorCode.InvalidToken);
            }

            if (tokenEntity.ExpiryAt < DateTime.UtcNow)
            {
                return RequestResult<ResetPasswordResponse>.Failure(ErrorCode.InvalidToken);
            }

            // 6. التوكن صالح - تغيير كلمة المرور
            var user = tokenEntity.User;

            // 7. إزالة كلمة المرور الحالية وإضافة الجديدة
            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                return RequestResult<ResetPasswordResponse>.Failure(ErrorCode.PasswordResetFailed);
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, request.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                return RequestResult<ResetPasswordResponse>.Failure(ErrorCode.PasswordResetFailed);
            }

            // 8. تعليم التوكن كمستخدم
            tokenEntity.IsUsed = true;
            tokenEntity.UpdatedAt = DateTime.UtcNow;

            // 9. SecurityStamp لإبطال جميع التوكنات القديمة
            await _userManager.UpdateSecurityStampAsync(user);

            await _unitOfWork.SaveChangesAsync();

            return RequestResult<ResetPasswordResponse>.Success(new ResetPasswordResponse
            {
                Success = true,
                Message = "Password has been reset successfully. Please login with your new password."
            });
        }

     


    }
}