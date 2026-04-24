using ExaminationSystem.Features.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.DeleteQuestion.Queries.CheckActiveAttempts
{
    public class CheckActiveAttemptsQueryHandler
            : IRequestHandler<CheckActiveAttemptsQuery, RequestResult<ActiveAttemptsDto>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public CheckActiveAttemptsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
          => _attemptRepo = attemptRepo;

        public async Task<RequestResult<ActiveAttemptsDto>> Handle(
            CheckActiveAttemptsQuery request, CancellationToken ct)
        {
            var activeAttemptsCount = await _attemptRepo
                .Get(a => a.QuizId == request.QuizId
                       && a.Status == QuizAttemptStatus.InProgress)
                .CountAsync(ct);

            return RequestResult<ActiveAttemptsDto>.Success(
                new ActiveAttemptsDto(request.QuizId, activeAttemptsCount > 0, activeAttemptsCount));
        }
    }
}