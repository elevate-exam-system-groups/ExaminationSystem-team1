using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Queries.GetTotalQuizzes
{
    public record GetTotalQuizzesQuery : IRequest<RequestResult<TotalQuizzesDto>>;
}
