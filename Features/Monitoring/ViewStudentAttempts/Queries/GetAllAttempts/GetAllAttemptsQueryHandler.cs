using ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.DTOs;
using ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.Specifications;
using ExaminationSystem.Features.Common.Paginated;
using ExaminationSystem.Features.Common.Paginated.DTOs;
using ExaminationSystem.Features.Common.Specifications;

namespace ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.Queries.GetAllAttempts
{
    public class GetAllAttemptsQueryHandler
     : IRequestHandler<GetAllAttemptsQuery, RequestResult<PaginatedResponseDto<AttemptDto>>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetAllAttemptsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
        {
            _attemptRepo = attemptRepo;
        }

        public async Task<RequestResult<PaginatedResponseDto<AttemptDto>>> Handle(
            GetAllAttemptsQuery request, CancellationToken ct)
        {

            var pageValidation = PaginationValidator.ValidatePage(request.Page);
            if (!pageValidation.IsSuccess)
                return RequestResult<PaginatedResponseDto<AttemptDto>>
                    .Failure(pageValidation.Message!, RequestErrorCode.ValidationError);

            var pageSizeValidation = PaginationValidator.ValidatePageSize(request.PageSize);
            if (!pageSizeValidation.IsSuccess)
                return RequestResult<PaginatedResponseDto<AttemptDto>>
                    .Failure(pageSizeValidation.Message!, RequestErrorCode.ValidationError);

            var statusValidation = PaginationValidator.ValidateStatus<QuizAttemptStatus>(request.Status);
            if (!statusValidation.IsSuccess)
                return RequestResult<PaginatedResponseDto<AttemptDto>>
                    .Failure(statusValidation.Message!, RequestErrorCode.ValidationError);

            var spec = new AttemptFilterSpecification(
                request.QuizId,
                request.StudentId,
                request.Status,
                request.SortBy,
                request.Order);

            var query = _attemptRepo
                .GetAll()
                .ApplySpecification(spec)
                .Select(a => new AttemptDto(
                    a.Id,
                    a.StudentId,
                    a.Student.FullName,
                    a.Student.Email,
                    a.QuizId,
                    a.Quiz.Title,
                    a.Status.ToString(),
                    a.Score,
                    a.IsPassed,
                    a.StartTime,
                    a.SubmittedAt
                ));

            var pagedList = await PaginatedList<AttemptDto>.CreateAsync(
               query, request.Page, request.PageSize, ct);

            var response = new PaginatedResponseDto<AttemptDto>(
                pagedList.Items,
                pagedList.TotalCount,
                pagedList.Page,
                pagedList.PageSize,
                pagedList.TotalPages,
                pagedList.HasPreviousPage,
                pagedList.HasNextPage
            );

            return RequestResult<PaginatedResponseDto<AttemptDto>>.Success(response);
        }
    }
}
