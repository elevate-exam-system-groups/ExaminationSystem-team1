namespace ExaminationSystem.Controllers.AdminManagementControllers.PerformanceAnalytics.ViewModels
{
    public record FailedQuestionVm(Guid QuestionId, string QuestionText, decimal FailRate, string QuizTitle);

}
