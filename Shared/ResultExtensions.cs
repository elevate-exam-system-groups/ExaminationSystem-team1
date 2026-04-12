using ExaminationSystem.Controllers.Shared.Enums;

namespace ExaminationSystem.Shared
{
    public record ErrorDetails(int StatusCode, string Message);

    public static class ResultExtensions
    {
        //  2. استخدام الـ Record داخل الـ Dictionary بدلاً من الـ Tuple
        private static readonly Dictionary<ResponseVmErrorCode, ErrorDetails> _errorMappings = new()
        {
            [ResponseVmErrorCode.None] = new ErrorDetails(200, null),
            [ResponseVmErrorCode.UserNotFound] = new ErrorDetails(404, "User not found"),
            [ResponseVmErrorCode.InvalidToken] = new ErrorDetails(400, "Token invalid or expired"),
            [ResponseVmErrorCode.PasswordResetFailed] = new ErrorDetails(400, "Password reset failed"),
            [ResponseVmErrorCode.EmailSendFailed] = new ErrorDetails(500, "Failed to send email"),
            [ResponseVmErrorCode.EmailAlreadyExists] = new ErrorDetails(409, "Email already registered"),
            [ResponseVmErrorCode.AccountNotVerified] = new ErrorDetails(403, "Account not verified"),
        };

        public static IActionResult ToHttpResponse<T>(this RequestResult<T> result) where T : class
        {
            if (result.IsSuccess)
            {
                // حالة خاصة: ForgotPassword
                if (result.Data is ForgotPasswordResponse forgotResponse && !forgotResponse.EmailSent)
                {
                    return new OkObjectResult(new { message = forgotResponse.Message });
                }

                return new OkObjectResult(result.Data);
            }

            // 3. استرجاع البيانات باستخدام الـ Record
            var mapping = _errorMappings.GetValueOrDefault(result.ResponseVmErrorCode, new ErrorDetails(400, "An error occurred"));

            // 4. استخدام الخصائص مباشرة بأسماء واضحة
            return new ObjectResult(new { message = mapping.Message })
            {
                StatusCode = mapping.StatusCode
            };
        }


    }
}