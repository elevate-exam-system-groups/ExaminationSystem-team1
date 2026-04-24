using ExaminationSystem.Features.Admin.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Admin.Queries.GetActiveUsersToday
{
    public record GetActiveUsersTodayQuery : IRequest<RequestResult<ActiveUsersTodayDto>>;
}

