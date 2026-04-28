namespace ExaminationSystem.Controllers.AdminManagementControllers.PerformanceAnalytics.ViewModels
{
    public record QuizPassRateVm(Guid QuizId, string QuizTitle, decimal PassRate, int TotalAttempts);

}
