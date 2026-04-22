namespace ExaminationSystem.Features.QuizModule.UnpublishQuiz.Queries
{
    public record CheckQuizHasCurrentAttemptsQueryRequest(Guid quizId) : IRequest<RequestResult<bool>>;

    public class CheckQuizHasCurrentAttemptsQueryRequestHandler
        : IRequestHandler<CheckQuizHasCurrentAttemptsQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptRepository;

        public CheckQuizHasCurrentAttemptsQueryRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptRepository)
        {
            _quizAttemptRepository = quizAttemptRepository;
        }

        public async Task<RequestResult<bool>> Handle(CheckQuizHasCurrentAttemptsQueryRequest request, CancellationToken cancellationToken)
        {
            var HasAttempts = await _quizAttemptRepository
                .Get(qa => qa.QuizId == request.quizId && qa.Status == QuizAttemptStatus.InProgress)
                .AnyAsync();

            if (HasAttempts)
            {
                return RequestResult<bool>
                    .Failure("Quiz has enrollments and cannot be unpublished.", RequestErrorCode.QuizHasEnrollments);
            }

            return RequestResult<bool>.Success(HasAttempts);
        }
    }


}
