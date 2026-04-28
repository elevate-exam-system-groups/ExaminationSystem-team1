using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs;
using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Specifications;

namespace ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Queries.GetAvgScoreByDiploma
{

    public class GetAvgScoreByDiplomaQueryHandler
        : IRequestHandler<GetAvgScoreByDiplomaQuery, RequestResult<List<DiplomaAvgScoreDto>>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetAvgScoreByDiplomaQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<List<DiplomaAvgScoreDto>>> Handle(
            GetAvgScoreByDiplomaQuery request, CancellationToken ct)
        {

            var spec = new AnalyticsFilterSpecification(request.From, request.To);

            var result = await _attemptRepo.GetAll()
                .Where(spec.Criteria)
                .GroupBy(a => new { a.Quiz.DiplomaId, a.Quiz.Diploma.Title })
                .Select(g => new DiplomaAvgScoreDto(
                    g.Key.DiplomaId,
                    g.Key.Title,
                    Math.Round(g.Average(a => (decimal?)a.Score) ?? 0, 1),
                    g.Count()
                ))
                .OrderByDescending(d => d.TotalAttempts)
                .ToListAsync(ct);

            return RequestResult<List<DiplomaAvgScoreDto>>.Success(result);
        }
    }
}

