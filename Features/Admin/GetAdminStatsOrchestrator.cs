using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin
{
    public record GetAdminStatsQuery : IRequest<RequestResult<AdminStatsResponse>>;
}
