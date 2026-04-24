namespace ExaminationSystem.Features.StudentDashboard.DTOs.OverallStats
{
    public record OverallStatsDto(
      int TotalQuizzesTaken,
      decimal AvgScore,
      decimal PassRate
    );
}
