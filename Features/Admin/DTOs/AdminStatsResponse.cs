namespace ExaminationSystem.Features.Admin.DTOs
{
    public record AdminStatsResponse(
    int TotalUsers,
    int ActiveUsersToday,
    int TotalQuizzes,
    int TotalAttempts,
    decimal AvgPassRate
    );
}
