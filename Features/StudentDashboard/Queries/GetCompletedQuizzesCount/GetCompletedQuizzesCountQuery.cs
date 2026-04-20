namespace ExaminationSystem.Features.StudentDashboard.Queries.GetCompletedQuizzesCount
{
    public record GetCompletedQuizzesCountQuery(List<Guid> CompletedQuizIds)
      : IRequest<RequestResult<Dictionary<Guid, int>>>;
}
