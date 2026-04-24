using ExaminationSystem.Features.StudentDashboard.DTOs.StudentDashboard;
using ExaminationSystem.Features.StudentDashboard.Orchestrator.EnrolledDiplomas;
using ExaminationSystem.Features.StudentDashboard.Queries.GetOverallStats;
using ExaminationSystem.Features.StudentDashboard.Queries.GetRecentAttempts;
using Microsoft.Extensions.Caching.Memory;

namespace ExaminationSystem.Features.StudentDashboard.Orchestrator.StudentDashboard
{
    public class GetStudentDashboardOrchestratorHandler
    : IRequestHandler<GetStudentDashboardOrchestrator, RequestResult<StudentDashboardResponseDto>>
    {

        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        public GetStudentDashboardOrchestratorHandler(IMediator mediator, IMemoryCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        public async Task<RequestResult<StudentDashboardResponseDto>> Handle(

            GetStudentDashboardOrchestrator request, CancellationToken ct)
        {

            var cacheKey = $"dashboard_student_{request.StudentId}";

            if (_cache.TryGetValue(cacheKey, out StudentDashboardResponseDto? cached))
                return RequestResult<StudentDashboardResponseDto>.Success(cached!);

            var diplomasResult = await _mediator.Send(new GetEnrolledDiplomasOrchestrator(request.StudentId), ct);
            var attemptsResult = await _mediator.Send(new GetRecentAttemptsQuery(request.StudentId), ct);
            var statsResult = await _mediator.Send(new GetOverallStatsQuery(request.StudentId), ct);


            if (!diplomasResult.IsSuccess || !attemptsResult.IsSuccess || !statsResult.IsSuccess)
                return RequestResult<StudentDashboardResponseDto>.Failure(
                    "Failed to load dashboard data",
                    RequestErrorCode.InternalServerError);

            var response = new StudentDashboardResponseDto(
                diplomasResult.Data.EnrolledDiplomas,
                attemptsResult.Data.Data!,
                statsResult.Data!
            );

            _cache.Set(cacheKey, response, TimeSpan.FromSeconds(60));

            return RequestResult<StudentDashboardResponseDto>.Success(response);
        }
    }
}
