namespace ExaminationSystem.Controllers.AttemptController.ViewModels
{
    public record AttemptAnswerVM
    (
        string StudentId,
        Guid AttemptId,
        Guid QuestionId,
        Guid OprionId
    );
}
