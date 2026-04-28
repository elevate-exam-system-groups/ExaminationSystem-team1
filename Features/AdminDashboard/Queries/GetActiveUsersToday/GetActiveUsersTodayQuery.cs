using ExaminationSystem.Features.AdminDashboard.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.AdminDashboard.Queries.GetActiveUsersToday
{
    public record GetActiveUsersTodayQuery : IRequest<RequestResult<ActiveUsersTodayDto>>;
}

