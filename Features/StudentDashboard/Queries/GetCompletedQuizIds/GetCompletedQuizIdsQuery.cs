using ExaminationSystem.Features.StudentDashboard.DTOs.Quiz;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetCompletedQuizIds
{
    public record GetCompletedQuizIdsQuery(string StudentId, List<Guid> DiplomaIds)
     : IRequest<RequestResult<CompletedQuizIdsDto>>;
}
