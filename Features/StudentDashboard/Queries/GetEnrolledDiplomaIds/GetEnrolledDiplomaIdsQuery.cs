namespace ExaminationSystem.Features.StudentDashboard.Queries.GetEnrolledDiplomaIds
{
    public record GetEnrolledDiplomaIdsQuery(string StudentId)
     : IRequest<RequestResult<List<Guid>>>;

}
