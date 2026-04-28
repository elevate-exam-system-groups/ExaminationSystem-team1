using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Orchestrator
{
    public record GetAnalyticsOrchestrator(DateTime? From = null, DateTime? To = null)
        : IRequest<RequestResult<AnalyticsResponseDto>>;
}
