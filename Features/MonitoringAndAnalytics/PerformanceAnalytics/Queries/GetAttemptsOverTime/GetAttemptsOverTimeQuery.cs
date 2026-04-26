using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Queries.GetAttemptsOverTime
{
    public record GetAttemptsOverTimeQuery(DateTime? From = null, DateTime? To = null)
        : IRequest<RequestResult<AnalyticsResponseDto>>;
}
