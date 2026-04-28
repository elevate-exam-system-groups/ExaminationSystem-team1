namespace ExaminationSystem.Controllers.AdminManagementControllers.PerformanceAnalytics.Filter
{
    public record AnalyticsFilterRequest(
     DateTime? From = null,
     DateTime? To = null
    );
}
