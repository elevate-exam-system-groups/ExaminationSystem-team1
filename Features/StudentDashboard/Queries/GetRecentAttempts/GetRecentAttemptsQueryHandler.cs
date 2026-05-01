using ExaminationSystem.Features.Common.Specifications;
using ExaminationSystem.Features.StudentDashboard.DTOs;
using ExaminationSystem.Features.StudentDashboard.Specifications;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetRecentAttempts
{
    public class GetRecentAttemptsQueryHandler
           : IRequestHandler<GetRecentAttemptsQuery, RequestResult<RecentAttemptResponseDto>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetRecentAttemptsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
        {
            _attemptRepo = attemptRepo;
        }

        public async Task<RequestResult<RecentAttemptResponseDto>> Handle(
            GetRecentAttemptsQuery request, CancellationToken ct)
        {

            var spec = new StudentAttemptSpecification(request.StudentId, onlyCompleted: false);

            var result = await _attemptRepo
                .GetAll()
                .ApplySpecification(spec)
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
                new RecentAttemptResponseDto(result));
        }
    }
}