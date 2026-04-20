namespace ExaminationSystem.Features.Admin.DTOs
{
    public record GlobalQuizStatsDto(int TotalQuizzes, int TotalAttempts, decimal AvgPassRate);
}
