namespace ExaminationSystem.Controllers.Monitoring_AnalyticsController.PerformanceAnalytics.ViewModels
{
    public record DiplomaAvgScoreVm(Guid DiplomaId, string DiplomaTitle, decimal AvgScore, int TotalAttempts);

}
