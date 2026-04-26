using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Queries.GetAvgScoreByDiploma
{
    public class GetAvgScoreByDiplomaQueryHandler
        : IRequestHandler<GetAvgScoreByDiplomaQuery, RequestResult<AnalyticsResponseDto>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetAvgScoreByDiplomaQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<AnalyticsResponseDto>> Handle(
            GetAvgScoreByDiplomaQuery request, CancellationToken ct)
        {
            // Build query with date filters
            var query = _attemptRepo.GetAll().Completed();

            if (request.From.HasValue)
                query = query.Where(a => a.StartTime >= request.From.Value);

            if (request.To.HasValue)
                query = query.Where(a => a.StartTime <= request.To.Value);

            // Execute aggregation
            var result = await query
                .GroupBy(a => new { a.Quiz.DiplomaId, a.Quiz.Diploma.Title })
                .Select(g => new DiplomaAvgScoreDto(
                    g.Key.DiplomaId,
                    g.Key.Title,
                    g.Any(a => a.Score.HasValue)
                        ? Math.Round(g.Average(a => a.Score ?? 0), 1)
                        : 0,
                    g.Count()
                ))
                .OrderByDescending(d => d.TotalAttempts)
                .ToListAsync(ct);

            // Return shared DTO with only AvgScoreByDiploma filled
            return RequestResult<AnalyticsResponseDto>.Success(
                new AnalyticsResponseDto(AvgScoreByDiploma: result));
        }
    }
}
