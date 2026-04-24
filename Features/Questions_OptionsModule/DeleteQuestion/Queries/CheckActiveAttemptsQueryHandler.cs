namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Queries
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
            var activeAttempts = await _attemptRepo
                .Get(a => a.QuizId == request.QuizId
                       && a.Status == QuizAttemptStatus.InProgress)
                .ToListAsync(ct);

            var hasActiveAttempts = activeAttempts.Any();

            if (hasActiveAttempts)
            {
                return RequestResult<ActiveAttemptsDto>.Failure(
                    $"Cannot delete question while there are {activeAttempts.Count} active quiz attempt(s).",
                    RequestErrorCode.Conflict);
            }

            var result = new ActiveAttemptsDto(request.QuizId, false, 0);

            return RequestResult<ActiveAttemptsDto>.Success(result);
        }
    }
}