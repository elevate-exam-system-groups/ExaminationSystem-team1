namespace ExaminationSystem.Controllers.StudentController.ViewModels
{
    public record OverallStatsVm(
     int TotalQuizzesTaken,
     decimal AvgScore,
     decimal PassRate
 );
}
