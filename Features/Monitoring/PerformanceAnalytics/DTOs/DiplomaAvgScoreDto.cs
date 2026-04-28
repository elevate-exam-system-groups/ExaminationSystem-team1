namespace ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs
{
    public record DiplomaAvgScoreDto(
        Guid DiplomaId,
        string DiplomaTitle,
        decimal AvgScore,
        int TotalAttempts
    );
}
