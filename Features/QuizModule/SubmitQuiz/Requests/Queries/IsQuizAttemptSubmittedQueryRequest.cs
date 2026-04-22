namespace ExaminationSystem.Features.QuizModule.SubmitQuiz.Requests.Queries
{
    public record IsQuizAttemptSubmittedQueryRequest(Guid quizAttemptId) : IRequest<RequestResult<bool>>;

    public class IsQuizAttemptSubmittedQueryRequestValidator : AbstractValidator<IsQuizAttemptSubmittedQueryRequest>
    {
        public IsQuizAttemptSubmittedQueryRequestValidator()
        {
            RuleFor(x => x.quizAttemptId)
                .NotEmpty().WithMessage("Quiz Attempt ID is required");
        }
    }

    public class IsQuizAttemptSubmittedQueryRequestHandler
        : IRequestHandler<IsQuizAttemptSubmittedQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<IsQuizAttemptSubmittedQueryRequest> _validator;
        public IsQuizAttemptSubmittedQueryRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<IsQuizAttemptSubmittedQueryRequest> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(IsQuizAttemptSubmittedQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                var result = RequestResult<bool>
                                            .Failure(validationErrors, RequestErrorCode.ValidationError);
                return result;
            }
            bool isSubmitted = _quizAttemptsRepository
               .Get(qa => qa.Id == request.quizAttemptId && qa.Status == QuizAttemptStatus.Submitted)
               .Any();

            if (isSubmitted)
            {
                RequestResult<bool>? result = RequestResult<bool>
                                    .Failure("Quiz attempt already submitted", RequestErrorCode.Conflict);
                return result;
            }

            return RequestResult<bool>.Success(isSubmitted);
        }
    }
}
