using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Queries
{
    public record GetGlobalAttemptStatsQuery : IRequest<RequestResult<AttemptStatsDto>>;
}
