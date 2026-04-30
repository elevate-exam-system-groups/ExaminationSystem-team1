namespace ExaminationSystem.Features.AttemptFeatures.SubmitQuizAttempt.Commands
{
    public record SetAttemptResultCommand(Guid AttemptId, decimal Score, bool IsPassed)
        : IRequest<RequestResult<bool>>;

    public class SetAttemptResultCommandValidator : AbstractValidator<SetAttemptResultCommand>
    {
        public SetAttemptResultCommandValidator()
        {
            RuleFor(x => x.AttemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
        }
    }

    public class SetAttemptResultCommandHandler
        : IRequestHandler<SetAttemptResultCommand, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<SetAttemptResultCommand> _validator;
        public SetAttemptResultCommandHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<SetAttemptResultCommand> validator)//, IMediator mediator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(SetAttemptResultCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<bool>
                      .Failure(validationErrors, RequestErrorCode.ValidationError);
            }

            var attemptResult = new QuizAttempt
            {
                Id = request.AttemptId,
                Score = request.Score,
                IsPassed = request.IsPassed,
                SubmittedAt = DateTime.UtcNow,
            };

            _quizAttemptsRepository.UpdateInclude(attemptResult,
                nameof(QuizAttempt.Score),
                nameof(QuizAttempt.IsPassed),
                nameof(QuizAttempt.SubmittedAt));

            await _quizAttemptsRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true);
        }
    }
}