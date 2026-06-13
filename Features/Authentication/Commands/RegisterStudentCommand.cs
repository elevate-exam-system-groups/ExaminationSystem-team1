using MediatR;

namespace ExaminationSystem.Features.Authentication.Commands
{
    public record RegisterStudentCommand(
        string FullName,
        string Email,
        string Password,
        string PhoneNumber) : IRequest<RegisterStudentResponse>;
}
