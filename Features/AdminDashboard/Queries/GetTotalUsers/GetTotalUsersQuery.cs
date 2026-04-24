using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Queries.GetTotalUsers
{
    public record GetTotalUsersQuery : IRequest<RequestResult<TotalUsersDto>>;
}
