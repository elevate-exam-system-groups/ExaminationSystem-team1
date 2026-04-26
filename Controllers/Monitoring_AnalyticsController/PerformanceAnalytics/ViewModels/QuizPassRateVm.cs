namespace ExaminationSystem.Controllers.Monitoring_AnalyticsController.PerformanceAnalytics.ViewModels
{
    public record QuizPassRateVm(Guid QuizId, string QuizTitle, decimal PassRate, int TotalAttempts);

}
