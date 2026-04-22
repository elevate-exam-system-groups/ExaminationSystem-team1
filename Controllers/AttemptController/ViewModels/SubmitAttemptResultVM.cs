namespace ExaminationSystem.Controllers.AttemptController.ViewModels
{
    public record SubmitAttemptResultVM
       (
        decimal Score,
        bool IsPassed,
        QuizAttemptStatus Status
        );
}
