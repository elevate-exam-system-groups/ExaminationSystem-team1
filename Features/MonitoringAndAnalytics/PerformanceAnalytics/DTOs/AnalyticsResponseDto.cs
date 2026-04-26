namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs
{
    public record AnalyticsResponseDto(
        List<QuizPassRateDto>? PassRateByQuiz = null,
        List<DiplomaAvgScoreDto>? AvgScoreByDiploma = null,
        List<AttemptsOverTimeDto>? AttemptsOverTime = null,
        List<FailedQuestionDto>? TopFailedQuestions = null
    );
}
