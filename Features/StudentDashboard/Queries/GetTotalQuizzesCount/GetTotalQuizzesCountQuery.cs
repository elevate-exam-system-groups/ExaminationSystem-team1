using ExaminationSystem.Features.StudentDashboard.DTOs.Quiz;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetTotalQuizzesCount
{
    public record GetTotalQuizzesCountQuery(List<Guid> DiplomaIds)
     : IRequest<RequestResult<TotalQuizzesCountDto>>;
}
