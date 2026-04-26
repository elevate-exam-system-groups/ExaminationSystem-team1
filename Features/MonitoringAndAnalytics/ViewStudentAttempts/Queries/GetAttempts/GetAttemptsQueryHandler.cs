using ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.DTOs;
using ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Extensions;
using ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Paginated;
using ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Specifications;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Queries.GetAttempts
{
    public class GetAttemptsQueryHandler
            : IRequestHandler<GetAttemptsQuery, RequestResult<PaginatedResponseDto<AttemptDto>>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetAttemptsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<PaginatedResponseDto<AttemptDto>>> Handle(
            GetAttemptsQuery request, CancellationToken ct)
        {
            // Build Specification
            var spec = new AttemptFilterSpecification(
                request.QuizId,
                request.StudentId,
                request.Status,
                request.SortBy,
                request.order
             );

            // Get Total Attempts Count for Pagination
            var totalCount = await _attemptRepo.CountAsync(spec);

            //  Get Items with Projection & Pagination
            var attempts = await _attemptRepo
                .GetAll()
                .ApplySpecification(spec)
                .ApplyPagination(request.Page, request.PageSize)
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
                )).ToListAsync(ct);

            //  Build Response 
            var response = new PaginatedResponseDto<AttemptDto>(
                attempts, totalCount, request.Page, request.PageSize);

            return RequestResult<PaginatedResponseDto<AttemptDto>>.Success(response);
        }

    }

}
