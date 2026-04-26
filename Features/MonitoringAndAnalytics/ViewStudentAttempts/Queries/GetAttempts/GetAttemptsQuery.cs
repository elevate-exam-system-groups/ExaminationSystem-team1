using ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.DTOs;
using ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Paginated;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Queries.GetAttempts
{
    public record GetAttemptsQuery(
        Guid? QuizId = null,
        string? StudentId = null,
        string? Status = null,
        string? SortBy = "submitted_at",
        string? order = "desc",
        int Page = 1,
        int PageSize = 20
    ) : IRequest<RequestResult<PaginatedResponseDto<AttemptDto>>>;
}
