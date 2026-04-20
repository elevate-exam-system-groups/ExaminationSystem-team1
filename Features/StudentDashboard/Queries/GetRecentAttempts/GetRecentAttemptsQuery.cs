using ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetRecentAttempts
{
    public record GetRecentAttemptsQuery(string StudentId)
     : IRequest<RequestResult<List<RecentAttemptDto>>>;
}
