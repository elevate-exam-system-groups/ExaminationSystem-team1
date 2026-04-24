using ExaminationSystem.Features.Admin.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Admin.Queries.GetTotalQuizzes
{
    public record GetTotalQuizzesQuery : IRequest<RequestResult<TotalQuizzesDto>>;
}
