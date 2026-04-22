namespace ExaminationSystem.Features.QuizModule.SubmitQuiz.Requests.Commands
{
    public record UpdateQuizAttemptStatusCommandRequest(Guid AttemptId, QuizAttemptStatus Status)
        : IRequest<RequestResult<bool>>;

    public class UpdateQuizAttemptStatusCommandRequestValidator : AbstractValidator<UpdateQuizAttemptStatusCommandRequest>
    {
        public UpdateQuizAttemptStatusCommandRequestValidator()
        {
            RuleFor(x => x.AttemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status value");
        }
    }

    public class UpdateQuizAttemptStatusCommandRequestHandler
        : IRequestHandler<UpdateQuizAttemptStatusCommandRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<UpdateQuizAttemptStatusCommandRequest> _validator;
        public UpdateQuizAttemptStatusCommandRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<UpdateQuizAttemptStatusCommandRequest> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(UpdateQuizAttemptStatusCommandRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                string? validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<bool>
                        .Failure(validationErrors, RequestErrorCode.ValidationError);

            }

            var attempt = _quizAttemptsRepository
                .Get(qa => qa.Id == request.AttemptId)
                .Select(qa => new QuizAttempt
                {
                    Id = qa.Id,
                    Status = request.Status
                })
                .FirstOrDefault();

            if (attempt == null)
            {
                return RequestResult<bool>.Failure("Quiz attempt not found.", RequestErrorCode.NotFound);
            }

            _quizAttemptsRepository.UpdateInclude(attempt, nameof(QuizAttempt.Status));

            await _quizAttemptsRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true);
        }
    }
}