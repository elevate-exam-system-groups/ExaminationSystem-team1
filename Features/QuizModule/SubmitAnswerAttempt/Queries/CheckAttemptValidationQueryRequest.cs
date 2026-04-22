namespace ExaminationSystem.Features.QuizModule.SubmitAnswerAttempt.Queries
{
    public record CheckAttemptValidationQueryRequest(Guid attemptId, string studentId) : IRequest<RequestResult<bool>>;

    public class CheckAttemptValidationQueryRequestValidator : AbstractValidator<CheckAttemptValidationQueryRequest>
    {
        public CheckAttemptValidationQueryRequestValidator()
        {
            RuleFor(x => x.attemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
            RuleFor(x => x.studentId)
                .NotEmpty().WithMessage("Student ID is required");
        }
    }
    public class CheckAttemptValidationQueryRequestHandler
        : IRequestHandler<CheckAttemptValidationQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<CheckAttemptValidationQueryRequest> _validator;
        public CheckAttemptValidationQueryRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<CheckAttemptValidationQueryRequest> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(CheckAttemptValidationQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                var result = RequestResult<bool>
                                            .Failure(validationErrors, RequestErrorCode.ValidationError);
                return result;
            }

            int QuizAttemptCount = _quizAttemptsRepository
                .Get(qa => qa.Id == request.attemptId &&
                 qa.StudentId == request.studentId).Count();

            var ValidAttempt = _quizAttemptsRepository
                .Get(qa => qa.Quiz.MaxAttempts > QuizAttemptCount &&
                 qa.Id == request.attemptId &&
                 qa.StudentId == request.studentId).Any();

            if (!ValidAttempt)
            {
                return RequestResult<bool>
                    .Failure("Invalid attempt", RequestErrorCode.Gone);
            }

            return RequestResult<bool>.Success(ValidAttempt);
        }
    }
}
