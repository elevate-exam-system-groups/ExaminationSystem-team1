namespace ExaminationSystem.Controllers.Monitoring_AnalyticsController.PerformanceAnalytics.ViewModels
{
    public record AnalyticsResponseVm(
        List<QuizPassRateVm> PassRateByQuiz,
        List<DiplomaAvgScoreVm> AvgScoreByDiploma,
        List<AttemptsOverTimeVm> AttemptsOverTime,
        List<FailedQuestionVm> TopFailedQuestions
    );
}
