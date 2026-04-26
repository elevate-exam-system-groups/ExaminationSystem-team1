namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs
{
    public record FailedQuestionDto(
        Guid QuestionId,
        string QuestionText,
        decimal FailRate,
        string QuizTitle
    );
}
