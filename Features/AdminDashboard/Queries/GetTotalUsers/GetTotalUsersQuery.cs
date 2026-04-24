using ExaminationSystem.Features.Admin.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Admin.Queries.GetTotalUsers
{
    public record GetTotalUsersQuery : IRequest<RequestResult<TotalUsersDto>>;
}
