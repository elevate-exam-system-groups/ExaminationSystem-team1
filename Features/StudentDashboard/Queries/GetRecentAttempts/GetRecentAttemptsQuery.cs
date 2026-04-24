using ExaminationSystem.Features.StudentDashboard.DTOs.Attempt;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetRecentAttempts
{
    public record GetRecentAttemptsQuery(string StudentId)
     : IRequest<RequestResult<RecentAttemptResponseDto>>;
}
