using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Queries.GetActiveUsersToday
{
    public record GetActiveUsersTodayQuery : IRequest<RequestResult<ActiveUsersTodayDto>>;
}

