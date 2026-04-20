using ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetCompletedQuizzesCount
{
    public record GetCompletedQuizzesCountQuery(List<Guid> CompletedQuizIds)
      : IRequest<RequestResult<CompletedQuizzesCountDto>>;
}
