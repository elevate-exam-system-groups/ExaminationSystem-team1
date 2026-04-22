namespace ExaminationSystem.Features.QuizModule.SubmitQuiz.Requests.Commands
{
    public record SaveAttemptResultCommandRequest(Guid attemptId, decimal score, bool IsPassed) : IRequest<RequestResult<bool>>;

    public class SaveAttemptResultCommandRequestValidator : AbstractValidator<SaveAttemptResultCommandRequest>
    {
        public SaveAttemptResultCommandRequestValidator()
        {
            RuleFor(x => x.attemptId)
                .NotEmpty().WithMessage("Attempt ID is required");

            RuleFor(x => x.score)
                .GreaterThanOrEqualTo(0).WithMessage("Score must be greater than or equal to 0");
        }
    }

    public class SaveAttemptResultCommandRequestHandler
        : IRequestHandler<SaveAttemptResultCommandRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<SaveAttemptResultCommandRequest> _validator;
        public SaveAttemptResultCommandRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<SaveAttemptResultCommandRequest> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(SaveAttemptResultCommandRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<bool>
                      .Failure(validationErrors, RequestErrorCode.ValidationError);
            }
            Guid attemptId = _quizAttemptsRepository
                               .GetById(request.attemptId)
                               .Select(qa => qa.Id)
                               .FirstOrDefault();

            if (attemptId == Guid.Empty)
                return RequestResult<bool>
                    .Failure("Quiz attempt not found", RequestErrorCode.NotFound);

            var attempt = new QuizAttempt
            {
                Id = attemptId,
                Score = request.score,
                IsPassed = request.IsPassed,
                Status = QuizAttemptStatus.Submitted,
                SubmittedAt = DateTime.UtcNow
            };

            _quizAttemptsRepository.UpdateInclude(attempt,
                nameof(QuizAttempt.Score),
                nameof(QuizAttempt.IsPassed),
                nameof(QuizAttempt.Status),
                nameof(QuizAttempt.SubmittedAt));

            await _quizAttemptsRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true);
        }
    }
}