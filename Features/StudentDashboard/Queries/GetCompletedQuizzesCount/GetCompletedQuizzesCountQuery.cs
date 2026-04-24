using ExaminationSystem.Features.StudentDashboard.DTOs.Quiz;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetCompletedQuizzesCount
{
    public record GetCompletedQuizzesCountQuery(List<Guid> CompletedQuizIds)
      : IRequest<RequestResult<CompletedQuizzesCountDto>>;
}
