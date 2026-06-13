namespace ExaminationSystem.Features.Authentication.Commands
{
    public record RegisterStudentResponse(bool IsSuccess, string Message, string UserId);
}
