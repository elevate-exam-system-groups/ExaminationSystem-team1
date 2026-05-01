using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.Monitoring.PerformanceAnalytics.Orchestrator.GetAnalytics
{
    public record GetAnalyticsOrchestrator(DateTime? From = null, DateTime? To = null)
        : IRequest<RequestResult<AnalyticsResponseDto>>;
}
