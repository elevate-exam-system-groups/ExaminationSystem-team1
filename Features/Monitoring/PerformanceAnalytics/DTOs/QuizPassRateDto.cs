namespace ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs
{
    public record QuizPassRateDto(
        Guid QuizId,
        string QuizTitle,
        decimal PassRate,
        int TotalAttempts
    );
}

