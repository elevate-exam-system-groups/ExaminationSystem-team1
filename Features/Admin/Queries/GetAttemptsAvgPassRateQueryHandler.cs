using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Queries
{
    public class GetAttemptsAvgPassRateQueryHandler
        : IRequestHandler<GetAttemptsAvgPassRateQuery, RequestResult<AttemptAvgPassRateDto>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetAttemptsAvgPassRateQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<AttemptAvgPassRateDto>> Handle(
            GetAttemptsAvgPassRateQuery request, CancellationToken ct)
        {
            var stats = await _attemptRepo
                .Get(a => a.Status != QuizAttemptStatus.InProgress && !a.isDeleted)
                .GroupBy(a => 1)
                .Select(g => new
                {
                    Total = g.Count(),
                    Passed = g.Count(a => a.IsPassed == true)
                })
                .FirstOrDefaultAsync(ct);

            var total = stats?.Total ?? 0;
            var passed = stats?.Passed ?? 0;
            var passRate = CalculatePassRate(total, passed);

            return RequestResult<AttemptAvgPassRateDto>.Success(new AttemptAvgPassRateDto(total, passRate));
        }

        private decimal CalculatePassRate(int total, int passed)
        {
            return total > 0
                  ? Math.Round((decimal)passed / total * 100, 1)
                  : 0;
        }
    }

}
