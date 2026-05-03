using ExaminationSystem.Controllers.AccountController.ViewModels.PasswordViewModels;
using ExaminationSystem.Features.Account.ForgetResetPassword.Forgot_ResetPassword;

namespace ExaminationSystem.Features.Account.ForgetResetPassword
{
    public static class ForgotPasswordEndpoint
    {
        public static void MapForgotPasswordEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v2/Auth/ForgotPassword", async (
                [FromBody] ForgotPasswordVM model,
                IMediator _mediator) =>
            {
                var command = new ForgotPasswordCommand(model.Email);
                var result = await _mediator.Send(command);

                if (!result.IsSuccess || result.Data == null)
                {
                    return Results.BadRequest(ResponseViewModel<ForgotPasswordResponseVM>.Failure(
                        result.Message ?? "An error occurred",
                        ResponseVmErrorCode.InternalServerError));
                }

                var responseVM = new ForgotPasswordResponseVM
                {
                    Message = result.Data.Message,
                    EmailSent = result.Data.EmailSent
                };

                return Results.Ok(ResponseViewModel<ForgotPasswordResponseVM>.Success(responseVM));
            })
            .WithTags("Auth")
            .WithName("ForgotPassword");
        }
    }
}
