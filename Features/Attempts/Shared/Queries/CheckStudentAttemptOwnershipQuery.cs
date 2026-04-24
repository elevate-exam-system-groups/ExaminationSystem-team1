namespace ExaminationSystem.Features.Attempts.Shared.Queries
{
    public record CheckStudentAttemptOwnershipQuery(Guid attemptId, string studentId)
        : IRequest<RequestResult<bool>>;

    public class CheckStudentAttemptOwnershipQueryValidator
        : AbstractValidator<CheckStudentAttemptOwnershipQuery>
    {
        public CheckStudentAttemptOwnershipQueryValidator()
        {
            RuleFor(x => x.attemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
            RuleFor(x => x.studentId)
                .NotEmpty().WithMessage("Student ID is required");
        }
    }

    public class CheckStudentAttemptOwnershipQueryHandler
        : IRequestHandler<CheckStudentAttemptOwnershipQuery, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Quiz> _quizAttemptsRepository;
        private readonly IValidator<CheckStudentAttemptOwnershipQuery> _validator;
        public CheckStudentAttemptOwnershipQueryHandler(IGeneralRepository<Quiz> quizAttemptsRepository, IValidator<CheckStudentAttemptOwnershipQuery> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(CheckStudentAttemptOwnershipQuery request, CancellationToken cancellationToken)
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

            var doesStudentOwnAttempt = await _quizAttemptsRepository
               .Get(qa => qa.Id == request.attemptId &&
                    qa.StudentId == request.studentId)
               .AnyAsync(cancellationToken);

            if (!doesStudentOwnAttempt)
            {
                return RequestResult<bool>
                    .Failure("Student does not own this attempt", RequestErrorCode.Forbidden);
            }

            return RequestResult<bool>.Success(doesStudentOwnAttempt);
        }
    }


}
