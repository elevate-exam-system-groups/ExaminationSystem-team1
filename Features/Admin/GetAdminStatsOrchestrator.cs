using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin
{
    public record GetAdminStatsOrchestrator : IRequest<RequestResult<AdminStatsResponse>>;
}
