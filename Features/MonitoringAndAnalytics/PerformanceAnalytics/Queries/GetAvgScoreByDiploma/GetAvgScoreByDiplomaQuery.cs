using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Queries.GetAvgScoreByDiploma
{
    public record GetAvgScoreByDiplomaQuery(DateTime? From = null, DateTime? To = null)
     : IRequest<RequestResult<AnalyticsResponseDto>>;
}
