using ExaminationSystem.Features.Admin.DTOs.ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Queries
{
    public record GetTotalUsersQuery : IRequest<RequestResult<TotalUsersDto>>;
}
