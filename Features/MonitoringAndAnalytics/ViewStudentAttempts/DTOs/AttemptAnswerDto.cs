namespace ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.DTOs
{
    public record AttemptAnswerDto(
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
