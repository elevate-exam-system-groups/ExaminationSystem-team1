namespace ExaminationSystem.Features.StudentDashboard.DTOs
{
    public record OverallStatsDto(
      int TotalQuizzesTaken,
      decimal AvgScore,
      decimal PassRate
    );
}
