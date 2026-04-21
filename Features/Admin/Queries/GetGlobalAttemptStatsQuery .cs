using ExaminationSystem.Features.Admin.DTOs.ExaminationSystem.Features.AdminDashboard.DTOs;

namespace ExaminationSystem.Features.Admin.Queries
{
    public record GetGlobalAttemptStatsQuery : IRequest<RequestResult<AttemptStatsDto>>;
}
