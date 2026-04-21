namespace ExaminationSystem.Controllers.AdminController.ViewModels
{
    public record AdminStatsVm(
     int TotalUsers,
     int ActiveUsersToday,
     int TotalQuizzes,
     int TotalAttempts,
     decimal AvgPassRate
 );
}
