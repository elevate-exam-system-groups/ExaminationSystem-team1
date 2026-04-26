using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Queries.GetAttemptsOverTime
{
    public class GetAttemptsOverTimeQueryHandler
      : IRequestHandler<GetAttemptsOverTimeQuery, RequestResult<AnalyticsResponseDto>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetAttemptsOverTimeQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<AnalyticsResponseDto>> Handle(
            GetAttemptsOverTimeQuery request, CancellationToken ct)
        {
            // Build query with date filters
            var query = _attemptRepo.GetAll().Completed();

            if (request.From.HasValue)
                query = query.Where(a => a.StartTime >= request.From.Value);

            if (request.To.HasValue)
                query = query.Where(a => a.StartTime <= request.To.Value);

            // Execute aggregation
            var result = await query
                .GroupBy(a => a.StartTime.Date)
                .Select(g => new AttemptsOverTimeDto(
                    g.Key,
                    g.Count()
                ))
                .OrderBy(a => a.Date)
                .ToListAsync(ct);

            // Return shared DTO with only AttemptsOverTime filled
            return RequestResult<AnalyticsResponseDto>.Success(
                new AnalyticsResponseDto(AttemptsOverTime: result));
        }
    }
}