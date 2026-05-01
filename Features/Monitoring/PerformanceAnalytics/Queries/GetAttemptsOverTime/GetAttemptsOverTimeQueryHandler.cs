using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs;
using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Specifications;

namespace ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Queries.GetAttemptsOverTime
{
    public class GetAttemptsOverTimeQueryHandler
     : IRequestHandler<GetAttemptsOverTimeQuery, RequestResult<List<AttemptsOverTimeDto>>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetAttemptsOverTimeQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
        {
            _attemptRepo = attemptRepo;
        }

        public async Task<RequestResult<List<AttemptsOverTimeDto>>> Handle(
            GetAttemptsOverTimeQuery request, CancellationToken ct)
        {

            var spec = new AnalyticsFilterSpecification(request.From, request.To);

            var result = await _attemptRepo.GetAll()
                .Where(spec.Criteria)
                .GroupBy(a => a.StartTime.Date)
                .Select(g => new AttemptsOverTimeDto(
                    g.Key,
                    g.Count()
                ))
                .OrderBy(a => a.Date)
                .ToListAsync(ct);

            return RequestResult<List<AttemptsOverTimeDto>>.Success(result);
        }
    }
}
