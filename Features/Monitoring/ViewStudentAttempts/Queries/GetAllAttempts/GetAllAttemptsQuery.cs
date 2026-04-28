using ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.DTOs;
using ExaminationSystem.Features.Common.Paginated.DTOs;

namespace ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.Queries.GetAllAttempts
{
    public record GetAllAttemptsQuery(
        Guid? QuizId = null,
        string? StudentId = null,
        string? Status = null,
        AttemptSortField SortBy = AttemptSortField.SubmittedAt,
        OrderDirection Order = OrderDirection.Desc,
        int Page = 1,
        int PageSize = 20
    ) : IRequest<RequestResult<PaginatedResponseDto<AttemptDto>>>;
}
