using ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetRecentAttempts
{
    public class GetRecentAttemptsQueryHandler
     : IRequestHandler<GetRecentAttemptsQuery, RequestResult<List<RecentAttemptDto>>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetRecentAttemptsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<List<RecentAttemptDto>>> Handle(
            GetRecentAttemptsQuery request, CancellationToken ct)
        {
            var attempts = await _attemptRepo
                .Get(a => a.StudentId == request.StudentId && !a.isDeleted)
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .Select(a => new RecentAttemptDto(
                    a.Id,
                    a.QuizId,
                    a.Quiz.Title,
                    a.Quiz.Diploma.Title,
                    a.Status.ToString(),
                    a.Score,
                    a.IsPassed,
                    a.SubmittedAt
                )).ToListAsync(ct);

            return RequestResult<List<RecentAttemptDto>>.Success(attempts);
        }
    }
}
