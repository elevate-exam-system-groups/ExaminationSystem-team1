namespace ExaminationSystem.Features.QuizModule.Shared
{
    public record CheckQuizTimerHasElapsedQueryRequest(Guid attemptId, string studentId)
        : IRequest<RequestResult<bool>>;

    public class CheckQuizTimerHasElapsedQueryRequestValidator
        : AbstractValidator<CheckQuizTimerHasElapsedQueryRequest>
    {
        public CheckQuizTimerHasElapsedQueryRequestValidator()
        {
            RuleFor(x => x.attemptId)
                .NotEmpty().WithMessage("Quiz ID is required");
            RuleFor(x => x.studentId)
                .NotEmpty().WithMessage("Student ID is required");
        }
    }

    public class CheckQuizTimerHasElapsedQueryRequestHandler
        : IRequestHandler<CheckQuizTimerHasElapsedQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<CheckQuizTimerHasElapsedQueryRequest> _validator;
        public CheckQuizTimerHasElapsedQueryRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<CheckQuizTimerHasElapsedQueryRequest> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(CheckQuizTimerHasElapsedQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                string? validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                var result = RequestResult<bool>
                                            .Failure(validationErrors, RequestErrorCode.ValidationError);
                return result;
            }
            bool hasQuizTimerElapsed = _quizAttemptsRepository
               .Get(qa => qa.Id == request.attemptId &&
                    qa.StudentId == request.studentId &&
                    qa.Status == QuizAttemptStatus.InProgress &&
                    DateTime.UtcNow > qa.DeadLine)
               .Any();

            if (hasQuizTimerElapsed)
            {
                return RequestResult<bool>
                    .Failure("Timer has elapsed", RequestErrorCode.Gone);
            }
            return RequestResult<bool>.Success(hasQuizTimerElapsed);
        }
    }
}
