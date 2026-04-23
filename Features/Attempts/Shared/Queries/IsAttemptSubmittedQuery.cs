namespace ExaminationSystem.Features.Attempts.Shared.Queries
{
    public record IsAttemptSubmittedQuery(Guid attemptId) : IRequest<RequestResult<bool>>;

    public class IsAttemptSubmittedQueryValidator : AbstractValidator<IsAttemptSubmittedQuery>
    {
        public IsAttemptSubmittedQueryValidator()
        {
            RuleFor(x => x.attemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
        }
    }

    public class IsAttemptSubmittedQueryHandler
        : IRequestHandler<IsAttemptSubmittedQuery, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<IsAttemptSubmittedQuery> _validator;
        public IsAttemptSubmittedQueryHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<IsAttemptSubmittedQuery> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(IsAttemptSubmittedQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string
                    .Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                var result = RequestResult<bool>
                                            .Failure(validationErrors, RequestErrorCode.ValidationError);
                return result;
            }

            var isAttemptSubmitted = _quizAttemptsRepository
               .Get(qa => qa.Id == request.attemptId &&
                     qa.Status == QuizAttemptStatus.Submitted)
               .Any();

            return RequestResult<bool>.Success(isAttemptSubmitted);
        }
    }

}
