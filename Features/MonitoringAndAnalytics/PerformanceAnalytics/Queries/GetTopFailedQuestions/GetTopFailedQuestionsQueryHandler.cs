using ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.PerformanceAnalytics.Queries.GetTopFailedQuestions
{
    public class GetTopFailedQuestionsQueryHandler
       : IRequestHandler<GetTopFailedQuestionsQuery, RequestResult<AnalyticsResponseDto>>
    {
        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;

        public GetTopFailedQuestionsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<AnalyticsResponseDto>> Handle(
            GetTopFailedQuestionsQuery request, CancellationToken ct)
        {
            // Build base query with date filters
            var baseQuery = _attemptRepo.GetAll().Completed();

            if (request.From.HasValue)
                baseQuery = baseQuery.Where(a => a.StartTime >= request.From.Value);

            if (request.To.HasValue)
                baseQuery = baseQuery.Where(a => a.StartTime <= request.To.Value);

            // Execute aggregation
            var stats = await baseQuery
                .SelectMany(a => a.UserAnswers)
                .Where(ua => !ua.IsDeleted)
                .GroupBy(ua => new { ua.QuestionId, ua.Question.Text, QuizTitle = ua.Question.Quiz.Title })
                .Select(g => new
                {
                    g.Key.QuestionId,
                    g.Key.Text,
                    g.Key.QuizTitle,
                    Total = g.Count(),
                    Failed = g.Count(ua => ua.IsCorrect == false)
                })
                .Where(x => x.Total >= 10)
                .ToListAsync(ct);

            var result = stats
                .Select(x => new FailedQuestionDto(
                    x.QuestionId,
                    x.Text,
                    x.Total > 0
                        ? Math.Round((decimal)x.Failed / x.Total * 100, 1)
                        : 0,
                    x.QuizTitle
                ))
                .Where(x => x.FailRate > 40)
                .OrderByDescending(x => x.FailRate)
                .Take(10)
                .ToList();

            // Return shared DTO with only TopFailedQuestions filled
            return RequestResult<AnalyticsResponseDto>.Success(
                new AnalyticsResponseDto(TopFailedQuestions: result));
        }
    }
}
