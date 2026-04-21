using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Queries
{
    public record GetTotalQuizzesQuery : IRequest<RequestResult<TotalQuizzesDto>>;
}
