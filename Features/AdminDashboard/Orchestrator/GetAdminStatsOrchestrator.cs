using ExaminationSystem.Features.AdminDashboard.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.AdminDashboard.Orchestrator
{
    public record GetAdminStatsOrchestrator : IRequest<RequestResult<AdminStatsResponseDto>>;
}
