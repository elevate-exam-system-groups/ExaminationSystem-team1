namespace ExaminationSystem.Features.QuizModule.SubmitAnswerAttempt.Requests
{
    public record CheckStudentOwnTheAttemptQueryRequest(Guid attemptId, string studentId)
        : IRequest<RequestResult<bool>>;

    public class CheckStudentOwnTheAttemptQueryRequestValidator
        : AbstractValidator<CheckStudentOwnTheAttemptQueryRequest>
    {
        public CheckStudentOwnTheAttemptQueryRequestValidator()
        {
            RuleFor(x => x.attemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
            RuleFor(x => x.studentId)
                .NotEmpty().WithMessage("Student ID is required");
        }
    }

    public class CheckStudentOwnTheAttemptQueryRequestHandler
        : IRequestHandler<CheckStudentOwnTheAttemptQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<CheckStudentOwnTheAttemptQueryRequest> _validator;
        public CheckStudentOwnTheAttemptQueryRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<CheckStudentOwnTheAttemptQueryRequest> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(CheckStudentOwnTheAttemptQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string
                    .Join(", ",
                    validationResult.Errors.Select(e => e.ErrorMessage));

                var result = RequestResult<bool>
                                            .Failure(validationErrors, RequestErrorCode.ValidationError);
                return result;
            }
            var doesStudentOwnTheAttempt = _quizAttemptsRepository
               .Get(qa => qa.Id == request.attemptId &&
                    qa.StudentId == request.studentId)
               .Any();

            if (!doesStudentOwnTheAttempt)
            {
                return RequestResult<bool>
                    .Failure("Student does not own this attempt", RequestErrorCode.Forbidden);
            }

            return RequestResult<bool>.Success(doesStudentOwnTheAttempt);
        }
    }


}
