using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Queries.GetPassRateByQuizQuery
{
    public record GetPassRateByQuizQuery(DateTime? From = null, DateTime? To = null)
      : IRequest<RequestResult<AnalyticsResponseDto>>;
}
