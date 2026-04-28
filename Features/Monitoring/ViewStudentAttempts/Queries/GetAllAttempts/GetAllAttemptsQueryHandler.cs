using ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.DTOs;
using ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.Specifications;
using ExaminationSystem.Features.Common.Paginated.DTOs;
using ExaminationSystem.Features.Common.Specifications;
using ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Paginated.ExaminationSystem.Features.MonitoringAndAnalytics.Paginated;

namespace ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.Queries.GetAllAttempts
{
    public class GetAllAttemptsQueryHandler
     : IRequestHandler<GetAllAttemptsQuery, RequestResult<PaginatedResponseDto<AttemptDto>>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetAllAttemptsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<PaginatedResponseDto<AttemptDto>>> Handle(
            GetAllAttemptsQuery request, CancellationToken ct)
        {

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
/*
 
 public class GetAttemptsQueryHandler
    : IRequestHandler<GetAttemptsQuery, RequestResult<PaginatedResponseDto<AttemptDto>>>
{
    private readonly IGeneralRepository<QuizAttempt> _attemptRepo;

    public GetAttemptsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
        => _attemptRepo = attemptRepo;

    public async Task<RequestResult<PaginatedResponseDto<AttemptDto>>> Handle(
        GetAttemptsQuery request, CancellationToken ct)
    {


        var spec = new AttemptFilterSpecification(
            request.QuizId, request.StudentId, request.Status, request.SortBy, request.Order);

        var query = _attemptRepo
            .GetAll()
            .ApplySpecification(spec)
            .ToDto(); // استخدام الـ Extension اللي عملناه

        var pagedList = await PaginatedList<AttemptDto>.CreateAsync(
           query, request.Page, request.PageSize, ct);

        // 4. تحويل النتيجة لـ Response DTO والرد
        return RequestResult<PaginatedResponseDto<AttemptDto>>.Success(pagedList.ToResponseDto());
    }
}
 
 */