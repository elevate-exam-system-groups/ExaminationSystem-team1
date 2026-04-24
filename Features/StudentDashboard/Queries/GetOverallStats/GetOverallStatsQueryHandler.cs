using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.StudentDashboard.DTOs.OverallStats;
using ExaminationSystem.Features.StudentDashboard.Helper;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetOverallStats
{
    public class GetOverallStatsQueryHandler
     : IRequestHandler<GetOverallStatsQuery, RequestResult<OverallStatsDto>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetOverallStatsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<OverallStatsDto>> Handle(
            GetOverallStatsQuery request, CancellationToken ct)
        {
            var stats = await _attemptRepo
                .Get(a => a.StudentId == request.StudentId)
                .Completed()
                .GroupBy(a => 1) 
                .Select(g => new
                {
                    Total = g.Count(),
                    AvgScore = g.Average(a => (decimal?)a.Score ?? 0),
                    PassedCount = g.Count(a => a.IsPassed == true)
                })
                .FirstOrDefaultAsync(ct);

            if (stats == null || stats.Total == 0)
                return RequestResult<OverallStatsDto>.Success(
                    new OverallStatsDto(0, 0, 0));


            var passRate = StatisticsHelper.CalculatePercentage(stats.Total , stats.PassedCount);
            var avgScore = StatisticsHelper.RoundScore(stats.AvgScore);

            return RequestResult<OverallStatsDto>.Success(new OverallStatsDto(
                stats.Total,
                avgScore,
                passRate
            ));
        }
    }

}
