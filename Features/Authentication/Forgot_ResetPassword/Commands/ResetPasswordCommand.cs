using ExaminationSystem.Features.Authentication.DTOs;

namespace ExaminationSystem.Features.Authentication.Forgot_ResetPassword.Commands
{
    public record ResetPasswordCommand(string Token, string NewPassword , string ConfirmNewPassword)
        : IRequest<RequestResult<ResetPasswordResponse>>;

}
