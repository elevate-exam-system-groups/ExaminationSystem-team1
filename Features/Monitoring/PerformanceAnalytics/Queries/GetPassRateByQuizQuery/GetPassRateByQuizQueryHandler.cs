using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs;
using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Specifications;
using ExaminationSystem.Features.Common.Helpers;

namespace ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Queries.GetPassRateByQuizQuery
{
    public class GetPassRateByQuizQueryHandler
         : IRequestHandler<GetPassRateByQuizQuery, RequestResult<List<QuizPassRateDto>>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetPassRateByQuizQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
        {
            _attemptRepo = attemptRepo;
        }

        public async Task<RequestResult<List<QuizPassRateDto>>> Handle(
            GetPassRateByQuizQuery request, CancellationToken ct)
        {

            var spec = new AnalyticsFilterSpecification(request.From, request.To);

            var result = await _attemptRepo.GetAll()
                .Where(spec.Criteria)
                .GroupBy(a => new { a.QuizId, a.Quiz.Title })
                .Select(g => new QuizPassRateDto(
                    g.Key.QuizId,
                    g.Key.Title,
                    StatisticsHelper.CalculatePercentage(g.Count(), g.Count(a => a.IsPassed == true)),
                    g.Count()
                ))
                .OrderByDescending(q => q.TotalAttempts)
                .ToListAsync(ct);

            return RequestResult<List<QuizPassRateDto>>.Success(result);
        }
    }
}