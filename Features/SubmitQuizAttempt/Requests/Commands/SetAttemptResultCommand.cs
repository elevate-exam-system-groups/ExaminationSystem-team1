using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.SubmitQuizAttempt.Requests.Commands
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
        // private readonly IMediator _mediator;
        private readonly IValidator<SetAttemptResultCommand> _validator;
        public SetAttemptResultCommandHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<SetAttemptResultCommand> validator)//, IMediator mediator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
            //_mediator = mediator;
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

            #region Command Call Query

            //var attemptResult = await _mediator
            //    .Send(new GetAttemptResultQuery(request.AttemptId), cancellationToken);

            //var attempt = new Quiz
            //{
            //    Id = request.AttemptId,
            //    Score = attemptResult.Data.score,
            //    IsPassed = attemptResult.Data.isPassed,
            //    Status = QuizAttemptStatus.Submitted,
            //    SubmittedAt = DateTime.UtcNow
            //};

            #endregion

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
                nameof(QuizAttempt.Status),
                nameof(QuizAttempt.SubmittedAt));

            await _quizAttemptsRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true);
        }
    }
}