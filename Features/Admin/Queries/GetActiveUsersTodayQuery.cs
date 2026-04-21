using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Queries
{
    public record GetActiveUsersTodayQuery : IRequest<RequestResult<ActiveUsersTodayDto>>;
}
