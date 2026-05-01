using ExaminationSystem.Features.AdminDashboard.DTOs;

namespace ExaminationSystem.Features.AdminDashboard.Queries.GetActiveUsersToday
{
    public record GetActiveUsersTodayQuery : IRequest<RequestResult<ActiveUsersTodayDto>>;
}

