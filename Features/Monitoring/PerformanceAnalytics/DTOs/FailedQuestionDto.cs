namespace ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs
{
    public record FailedQuestionDto(
        Guid QuestionId,
        string QuestionText,
        decimal FailRate,
        string QuizTitle
    );
}
