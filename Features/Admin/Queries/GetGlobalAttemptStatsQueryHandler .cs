using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Queries
{
    public class GetGlobalAttemptStatsQueryHandler 
        : IRequestHandler<GetGlobalAttemptStatsQuery, RequestResult<AttemptStatsDto>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetGlobalAttemptStatsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo) 
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<AttemptStatsDto>> Handle
            (GetGlobalAttemptStatsQuery request, CancellationToken ct)
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
            var passRate = total > 0
                ? Math.Round((decimal)stats!.Passed / total * 100, 1)
                : 0;

            return RequestResult<AttemptStatsDto>.Success(new AttemptStatsDto(total, passRate));
        }
    }
}
}

