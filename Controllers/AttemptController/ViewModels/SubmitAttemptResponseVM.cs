namespace ExaminationSystem.Controllers.AttemptController.ViewModels
{
    public record SubmitAttemptResponseVM(decimal Score, bool IsPassed, QuizAttemptStatus Status);


}
