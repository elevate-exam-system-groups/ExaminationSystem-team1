using ExaminationSystem.Features.StudentDashboard.DTOs;

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
            var attempts = await _attemptRepo
                .Get(a => a.StudentId == request.StudentId
                       && a.Status != QuizAttemptStatus.InProgress
                       && !a.isDeleted)
                .Select(a => new { a.Score, a.IsPassed })
                .ToListAsync(ct);

            if (!attempts.Any())
                return RequestResult<OverallStatsDto>.Success(new OverallStatsDto(0, 0, 0));

            var total = attempts.Count;
            var avgScore = attempts.Average(a => a.Score ?? 0);
            var passedCount = attempts.Count(a => a.IsPassed == true);
            var passRate = (decimal)passedCount / total * 100;

            return RequestResult<OverallStatsDto>.Success(new OverallStatsDto(
                total,
                Math.Round(avgScore, 1),
                Math.Round(passRate, 1)
            ));
        }
    }
}
