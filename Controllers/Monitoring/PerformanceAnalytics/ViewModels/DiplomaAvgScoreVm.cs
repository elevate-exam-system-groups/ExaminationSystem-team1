namespace ExaminationSystem.Controllers.AdminManagementControllers.PerformanceAnalytics.ViewModels
{
    public record DiplomaAvgScoreVm(Guid DiplomaId, string DiplomaTitle, decimal AvgScore, int TotalAttempts);

}
