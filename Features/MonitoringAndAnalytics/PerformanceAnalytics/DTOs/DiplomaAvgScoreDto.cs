namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs
{
    public record DiplomaAvgScoreDto(
        Guid DiplomaId,
        string DiplomaTitle,
        decimal AvgScore,
        int TotalAttempts
    );
}
