namespace ExaminationSystem.Features.StudentDashboard.Queries.GetTotalQuizzesCount
{
    public record GetTotalQuizzesCountQuery(List<Guid> DiplomaIds)
     : IRequest<RequestResult<Dictionary<Guid, int>>>;
}
