using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.StudentDashboard.DTOs.Attempt;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetRecentAttempts
{
    public class GetRecentAttemptsQueryHandler
     : IRequestHandler<GetRecentAttemptsQuery, RequestResult<RecentAttemptResponseDto>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetRecentAttemptsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<RecentAttemptResponseDto>> Handle(
            GetRecentAttemptsQuery request, CancellationToken ct)
        {
            var attempts = await _attemptRepo
                .Get(a => a.StudentId == request.StudentId && !a.IsDeleted)
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

            return RequestResult<RecentAttemptResponseDto>.Success(
                new RecentAttemptResponseDto(attempts));
        }
    }
}
