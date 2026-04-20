using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Queries
{
    public class GetGlobalQuizStatsQueryHandler
       : IRequestHandler<GetGlobalQuizStatsQuery, GlobalQuizStatsDto>
    {
        private readonly IGeneralRepository<Quiz> _quizRepo;
        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;

        public GetGlobalQuizStatsQueryHandler(
            IGeneralRepository<Quiz> quizRepo,
            IGeneralRepository<QuizAttempt> attemptRepo)
        {
            _quizRepo = quizRepo;
            _attemptRepo = attemptRepo;
        }

        public async Task<GlobalQuizStatsDto> Handle(
            GetGlobalQuizStatsQuery request, CancellationToken ct)
        {
            // Run in parallel
            var quizzesTask = _quizRepo
                .Get(q => !q.isDeleted && q.Status == QuizStatus.Published)
                .CountAsync(ct);

            var attemptsTask = _attemptRepo
                .Get(a => !a.isDeleted && a.Status != QuizAttemptStatus.InProgress)
                .GroupBy(a => 1)
                .Select(g => new {
                    TotalAttempts = g.Count(),
                    PassedCount = g.Count(x => x.IsPassed == true)
                })
                .FirstOrDefaultAsync(ct);

            await Task.WhenAll(quizzesTask, attemptsTask);

            var totalQuizzes = await quizzesTask;
            var stats = await attemptsTask;

            var passRate = stats?.TotalAttempts > 0
                ? Math.Round((decimal)stats.PassedCount / stats.TotalAttempts * 100, 1)
                : 0;

            return new GlobalQuizStatsDto(
                totalQuizzes,
                stats?.TotalAttempts ?? 0,
                passRate
            );
        }
    }
}

