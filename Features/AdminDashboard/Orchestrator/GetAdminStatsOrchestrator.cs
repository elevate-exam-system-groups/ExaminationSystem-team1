using ExaminationSystem.Features.Admin.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Admin.Orchestrator
{
    public record GetAdminStatsOrchestrator : IRequest<RequestResult<AdminStatsResponseDto>>;
}
