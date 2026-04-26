namespace ExaminationSystem.Controllers.Monitoring_AnalyticsController.PerformanceAnalytics.ViewModels
{
    public record FailedQuestionVm(Guid QuestionId, string QuestionText, decimal FailRate, string QuizTitle);

}
