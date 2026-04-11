using ExaminationSystem.Features.Authentication.DTOs;

namespace ExaminationSystem.Features.Authentication.ForgotPassword.Commands
{
    public record ForgotPasswordCommand(string Email) : IRequest<RequestResult<ForgotPasswordResponse>>;
}
