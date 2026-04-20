using ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetTotalQuizzesCount
{
    public record GetTotalQuizzesCountQuery(List<Guid> DiplomaIds)
     : IRequest<RequestResult<TotalQuizzesCountDto>>;
}
