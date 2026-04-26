namespace ExaminationSystem.Controllers.Monitoring_Analytics.ViewStudentAttempts.ViewModels
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
