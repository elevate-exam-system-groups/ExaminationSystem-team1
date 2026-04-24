using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Orchestrator
{
    public record GetAdminStatsOrchestrator : IRequest<RequestResult<AdminStatsResponseDto>>;
}
