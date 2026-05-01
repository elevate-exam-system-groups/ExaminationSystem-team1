using ExaminationSystem.Features.AdminDashboard.DTOs;

namespace ExaminationSystem.Features.AdminDashboard.Queries.GetTotalUsers
{
    public record GetTotalUsersQuery() : IRequest<RequestResult<TotalUsersDto>>;
}
