using ExaminationSystem.Features.Account.ForgetResetPassword.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Shared
{
    public record ErrorDetails(int StatusCode, string Message);

    public static class ResultExtensions
    {
        // 2. استخدام الـ Record داخل الـ Dictionary بدلاً من الـ Tuple
        private static readonly Dictionary<ErrorCode, ErrorDetails> _errorMappings = new()
        {
            [ErrorCode.None] = new ErrorDetails(200, null),
            [ErrorCode.UserNotFound] = new ErrorDetails(404, "User not found"),
            [ErrorCode.InvalidToken] = new ErrorDetails(400, "Token invalid or expired"),
            [ErrorCode.PasswordResetFailed] = new ErrorDetails(400, "Password reset failed"),
            [ErrorCode.EmailSendFailed] = new ErrorDetails(500, "Failed to send email"),
            [ErrorCode.EmailAlreadyExists] = new ErrorDetails(409, "Email already registered"),
            [ErrorCode.AccountNotVerified] = new ErrorDetails(403, "Account not verified"),
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
            var mapping = _errorMappings.GetValueOrDefault(result.ErrorCode, new ErrorDetails(400, "An error occurred"));

            // 4. استخدام الخصائص مباشرة بأسماء واضحة
            return new ObjectResult(new { message = mapping.Message })
            {
                StatusCode = mapping.StatusCode
            };
        }


    }
}