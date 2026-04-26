using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Orchestrator
{
    public record GetAnalyticsOrchestrator(DateTime? From = null,  DateTime? To = null) 
        : IRequest<RequestResult<AnalyticsResponseDto>>;
    
}
