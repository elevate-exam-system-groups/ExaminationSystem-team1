namespace ExaminationSystem.Features.AdminDashboard.DTOs
{
    public record AdminStatsResponseDto(
    int TotalUsers,
    int ActiveUsersToday,
    int TotalQuizzes,
    int TotalAttempts,
    decimal AvgPassRate
    );
}
