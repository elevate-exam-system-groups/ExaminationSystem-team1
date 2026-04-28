namespace ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs
{
    public record AttemptsOverTimeDto(
        DateTime Date,
        int Count
    );
}
