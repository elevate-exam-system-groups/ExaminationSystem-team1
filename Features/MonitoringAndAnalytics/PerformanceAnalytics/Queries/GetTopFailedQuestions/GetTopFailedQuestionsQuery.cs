using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Queries.GetTopFailedQuestions
{
    public record GetTopFailedQuestionsQuery(DateTime? From = null, DateTime? To = null)
    : IRequest<RequestResult<AnalyticsResponseDto>>;
}
