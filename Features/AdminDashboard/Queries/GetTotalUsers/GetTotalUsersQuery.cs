using ExaminationSystem.Features.AdminDashboard.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.AdminDashboard.Queries.GetTotalUsers
{
    public record GetTotalUsersQuery : IRequest<RequestResult<TotalUsersDto>>;
}
