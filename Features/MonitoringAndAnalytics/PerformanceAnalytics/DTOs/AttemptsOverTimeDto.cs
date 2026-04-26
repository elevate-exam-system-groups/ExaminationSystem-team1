namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs
{
    public record AttemptsOverTimeDto(
        DateTime Date,
        int Count
    );
}
