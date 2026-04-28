namespace ExaminationSystem.Controllers.AdminManagementControllers.ViewStudentAttempts.ViewModels
{
    public record AttemptAnswerVm(
      Guid QuestionId,
      string QuestionText,
      int OrderIndex,
      Guid SelectedOptionId,
      string SelectedOptionText,
      bool? IsCorrect,
      Guid CorrectOptionId,
      string CorrectOptionText,
      string? Explanation
  );
}
