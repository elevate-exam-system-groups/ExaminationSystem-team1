using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Queries.GetPassRateByQuizQuery
{
    public class GetPassRateByQuizQueryHandler
      : IRequestHandler<GetPassRateByQuizQuery, RequestResult<AnalyticsResponseDto>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetPassRateByQuizQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<AnalyticsResponseDto>> Handle(
            GetPassRateByQuizQuery request, CancellationToken ct)
        {
            // Build query with date filters
            var query = _attemptRepo.GetAll().Completed();

            if (request.From.HasValue)
                query = query.Where(a => a.StartTime >= request.From.Value);

            if (request.To.HasValue)
                query = query.Where(a => a.StartTime <= request.To.Value);

            // Execute aggregation
            var result = await query
                .GroupBy(a => new { a.QuizId, a.Quiz.Title })
                .Select(g => new QuizPassRateDto(
                    g.Key.QuizId,
                    g.Key.Title,
                    g.Count() > 0
                        ? Math.Round((decimal)g.Count(a => a.IsPassed == true) / g.Count() * 100, 1)
                        : 0,
                    g.Count()

                )).OrderByDescending(q => q.TotalAttempts)
                .ToListAsync(ct);

            // Return shared DTO with only PassRateByQuiz filled
            return RequestResult<AnalyticsResponseDto>.Success(
                new AnalyticsResponseDto(PassRateByQuiz: result));
        }
    }
}


