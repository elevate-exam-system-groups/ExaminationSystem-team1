namespace ExaminationSystem.Features.AnswerQuestion.Requests
{
    public record CheckInProgressQuizAttemptStatusQuerRequest(Guid attemptId, string studentId)
        : IRequest<RequestResult<bool>>;

    public class CheckInProgressQuizAttemptStatusQuerRequestValidator : AbstractValidator<CheckInProgressQuizAttemptStatusQuerRequest>
    {
        public CheckInProgressQuizAttemptStatusQuerRequestValidator()
        {
            RuleFor(x => x.attemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
            RuleFor(x => x.studentId)
                .NotEmpty().WithMessage("Student ID is required");
        }
    }

    public class CheckInProgressQuizAttemptStatusQuerRequestHandler
        : IRequestHandler<CheckInProgressQuizAttemptStatusQuerRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<CheckInProgressQuizAttemptStatusQuerRequest> _validator;
        public CheckInProgressQuizAttemptStatusQuerRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<CheckInProgressQuizAttemptStatusQuerRequest> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(CheckInProgressQuizAttemptStatusQuerRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                var result = RequestResult<bool>
                                            .Failure(validationErrors, RequestErrorCode.ValidationError);
                return result;
            }
            var hasInProgressAttempt = _quizAttemptsRepository
               .Get(qa => qa.Id == request.attemptId &&
                    qa.StudentId == request.studentId &&
                    qa.Status == QuizAttemptStatus.InProgress)
               .Any();

            if (!hasInProgressAttempt)
            {
                return RequestResult<bool>
                .Failure("Attempt is already submitted or expired", RequestErrorCode.Conflict);
            }
            return RequestResult<bool>.Success(hasInProgressAttempt);
        }
    }
}