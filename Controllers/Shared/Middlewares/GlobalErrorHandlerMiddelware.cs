using ExaminationSystem.Controllers.Shared;
using ExaminationSystem.Controllers.Shared.Enums;

namespace ExaminationSystem.Controllers.Shared.Middlewares
{
    public class GlobalErrorHandlerMiddelware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;

                var (errorCode, statusCode) = GetErrorCodeFromException(ex);

                var response =
                      ResponseViewModel<Exception>.Failure(errorMessage, errorCode);


                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsJsonAsync(response);
            }
        }


        private (ResponseVmErrorCode errorCode, int statusCode) GetErrorCodeFromException(Exception ex)
        {
            return ex switch
            {
                UnauthorizedAccessException => (ResponseVmErrorCode.Unauthorized, 401),
                KeyNotFoundException => (ResponseVmErrorCode.UserNotFound, 404),
                ArgumentException => (ResponseVmErrorCode.InvalidToken, 400),
                InvalidOperationException => (ResponseVmErrorCode.PasswordResetFailed, 400),
                _ => (ResponseVmErrorCode.InternalServerError, 500)
            };
        }
    }
}
