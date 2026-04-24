namespace ExaminationSystem.Features.Admin.DTOs
{
    public record AdminStatsResponseDto(
    int TotalUsers,
    int ActiveUsersToday,
    int TotalQuizzes,
    int TotalAttempts,
    decimal AvgPassRate
    );
}
