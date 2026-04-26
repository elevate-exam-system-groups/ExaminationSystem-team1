namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs
{
    public record QuizPassRateDto(
        Guid QuizId,
        string QuizTitle,
        decimal PassRate,
        int TotalAttempts
    );
}

