using ExaminationSystem.Features.AdminDashboard.DTOs;

namespace ExaminationSystem.Features.AdminDashboard.Orchestrator
{
    public record GetAdminStatsOrchestrator() : IRequest<RequestResult<AdminStatsResponseDto>>;
}
