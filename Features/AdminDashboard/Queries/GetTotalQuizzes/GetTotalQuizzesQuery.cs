using ExaminationSystem.Features.AdminDashboard.DTOs;

namespace ExaminationSystem.Features.AdminDashboard.Queries.GetTotalQuizzes
{
    public record GetTotalQuizzesQuery() : IRequest<RequestResult<TotalQuizzesDto>>;
}
