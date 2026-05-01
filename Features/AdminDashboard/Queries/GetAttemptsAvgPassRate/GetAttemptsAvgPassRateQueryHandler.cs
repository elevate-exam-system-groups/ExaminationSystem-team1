using ExaminationSystem.Features.AdminDashboard.DTOs;
using ExaminationSystem.Features.Common.Helpers;

namespace ExaminationSystem.Features.AdminDashboard.Queries.GetAttemptsAvgPassRate
{
    public class GetAttemptsAvgPassRateQueryHandler
        : IRequestHandler<GetAttemptsAvgPassRateQuery, RequestResult<AttemptAvgPassRateDto>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetAttemptsAvgPassRateQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
        {
            _attemptRepo = attemptRepo;
        }

        public async Task<RequestResult<AttemptAvgPassRateDto>> Handle(
            GetAttemptsAvgPassRateQuery request, CancellationToken ct)
        {
            var stats = await _attemptRepo
                .Get(a => a.Status != QuizAttemptStatus.InProgress)
                .GroupBy(a => 1)
                .Select(g => new
                {
                    Total = g.Count(),
                    Passed = g.Count(a => a.IsPassed == true)

                }).FirstOrDefaultAsync(ct);

            var total = stats?.Total ?? 0;
            var passed = stats?.Passed ?? 0;
            var passRate = StatisticsHelper.CalculatePercentage(total, passed);

            return RequestResult<AttemptAvgPassRateDto>.Success(
                new AttemptAvgPassRateDto(total, passRate));
        }

    }
}
