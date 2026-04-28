using ExaminationSystem.Features.AdminDashboard.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.AdminDashboard.Queries.GetTotalQuizzes
{
    public record GetTotalQuizzesQuery : IRequest<RequestResult<TotalQuizzesDto>>;
}
