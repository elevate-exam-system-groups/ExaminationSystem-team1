using ExaminationSystem.Controllers.AccountController.ViewModels.PasswordViewModels;
using ExaminationSystem.Features.Account.ForgetResetPassword.Forgot_ResetPassword;

namespace ExaminationSystem.Features.Account.ForgetResetPassword
{
    public static class ResetPasswordEndpoint
    {
        public static void MapResetPasswordEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v2/Auth/ResetPassword", async (
                [FromBody] ResetPasswordVM model,
                IMediator _mediator) =>
            {
                var command = new ResetPasswordCommand(
                    model.Email,
                    model.Token,
                    model.NewPassword,
                    model.ConfirmNewPassword);

                var result = await _mediator.Send(command);

                if (!result.IsSuccess || result.Data == null)
                {
                    return Results.BadRequest(ResponseViewModel<ResetPasswordResponseVM>.Failure(
                        result.Message ?? "Password reset failed",
                        ResponseVmErrorCode.PasswordResetFailed));
                }

                var responseVM = new ResetPasswordResponseVM
                {
                    Success = result.Data.Success,
                    Message = result.Data.Message
                };

                return Results.Ok(ResponseViewModel<ResetPasswordResponseVM>.Success(responseVM));
            })
            .WithTags("Auth")
            .WithName("ResetPassword");
        }
    }
}
