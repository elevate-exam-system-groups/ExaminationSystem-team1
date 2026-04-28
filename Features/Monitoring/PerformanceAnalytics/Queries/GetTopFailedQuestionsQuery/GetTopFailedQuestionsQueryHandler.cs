using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs;
using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Specifications;
using ExaminationSystem.Features.Common.Helpers;

namespace ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Queries.GetTopFailedQuestionsQuery
{
    public class GetTopFailedQuestionsQueryHandler
       : IRequestHandler<GetTopFailedQuestionsQuery, RequestResult<List<FailedQuestionDto>>>
    {
        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;

        private const int MinAttemptsThreshold = 10;
        private const decimal HighFailRateThreshold = 40.0m;

        public GetTopFailedQuestionsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<List<FailedQuestionDto>>> Handle(
            GetTopFailedQuestionsQuery request, CancellationToken ct)
        {
            var spec = new AnalyticsFilterSpecification(request.From, request.To);

            var failedQuestions = await _attemptRepo.GetAll()
                .Where(spec.Criteria)
                .SelectMany(a => a.UserAnswers)
                .Where(ua => !ua.IsDeleted) 
                .GroupBy(ua => new
                {
                    ua.QuestionId,
                    ua.Question.Text,
                    QuizTitle = ua.Question.Quiz.Title
                })
                .Where(g => g.Count() >= MinAttemptsThreshold) 
                .Select(g => new FailedQuestionDto(
                    g.Key.QuestionId,
                    g.Key.Text,
                    StatisticsHelper.CalculatePercentage(g.Count(), g.Count(x => !x.IsCorrect == false)),
                    g.Key.QuizTitle
                ))
                .Where(x => x.FailRate > HighFailRateThreshold) 
                .OrderByDescending(x => x.FailRate)
                .Take(10)
                .ToListAsync(ct);

            return RequestResult<List<FailedQuestionDto>>.Success(failedQuestions);
        }
    }
}