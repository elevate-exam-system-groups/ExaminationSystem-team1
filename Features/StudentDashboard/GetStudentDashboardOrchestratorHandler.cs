using ExaminationSystem.Features.StudentDashboard.DTOs;
using ExaminationSystem.Features.StudentDashboard.ExaminationSystem.Features.StudentDashboard.Queries;
using ExaminationSystem.Features.StudentDashboard.Queries;
using ExaminationSystem.Features.StudentDashboard.Queries.GetOverallStats;
using ExaminationSystem.Features.StudentDashboard.Queries.GetRecentAttempts;
using ExaminationSystem.Features.StudentDashboard.Test;
using Microsoft.Extensions.Caching.Memory;

namespace ExaminationSystem.Features.StudentDashboard
{
    public class GetStudentDashboardOrchestratorHandler
    : IRequestHandler<GetStudentDashboardQuery, RequestResult<StudentDashboardResponse>>
    {

        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        public GetStudentDashboardOrchestratorHandler(IMediator mediator, IMemoryCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        public async Task<RequestResult<StudentDashboardResponse>> Handle(

            GetStudentDashboardQuery request, CancellationToken ct)
        {
            var cacheKey = $"dashboard_student_{request.StudentId}";

            if (_cache.TryGetValue(cacheKey, out StudentDashboardResponse? cached))
                return RequestResult<StudentDashboardResponse>.Success(cached!);

            var diplomasTask = await _mediator.Send(new GetEnrolledDiplomasCommand(request.StudentId), ct);
            var attemptsTask = await _mediator.Send(new GetRecentAttemptsQuery(request.StudentId), ct);
            var statsTask = await _mediator.Send(new GetOverallStatsQuery(request.StudentId), ct);


            var response = new StudentDashboardResponse(
                diplomasTask.Data!,
                attemptsTask.Data!,
                statsTask.Data!
            );

            _cache.Set(cacheKey, response, TimeSpan.FromSeconds(60));

            return RequestResult<StudentDashboardResponse>.Success(response);
        }
    }
}
