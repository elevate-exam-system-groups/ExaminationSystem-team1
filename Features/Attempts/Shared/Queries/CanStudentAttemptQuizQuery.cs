namespace ExaminationSystem.Features.Attempts.Shared.Queries
{
    public record CanStudentAttemptQuizQuery(Guid QuizId, string studentId) : IRequest<RequestResult<bool>>;

    public class CanStudentAttemptQuizQueryValidator : AbstractValidator<CanStudentAttemptQuizQuery>
    {
        public CanStudentAttemptQuizQueryValidator()
        {
            RuleFor(x => x.QuizId)
                .NotEmpty().WithMessage("Quiz ID is required");
            RuleFor(x => x.studentId)
                .NotEmpty().WithMessage("Student ID is required");
        }
    }
    public class CanStudentAttemptQuizQueryHandler
        : IRequestHandler<CanStudentAttemptQuizQuery, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Quiz> _quizAttemptsRepository;
        private readonly IValidator<CanStudentAttemptQuizQuery> _validator;
        public CanStudentAttemptQuizQueryHandler(IGeneralRepository<Quiz> quizAttemptsRepository, IValidator<CanStudentAttemptQuizQuery> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(CanStudentAttemptQuizQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                var result = RequestResult<bool>
                                            .Failure(validationErrors, RequestErrorCode.ValidationError);
                return result;
            }

            var maxAttempts = await _quizAttemptsRepository
                                 .Get(qa => qa.QuizId == request.QuizId)
                                 .Select(qa => qa.Quiz.MaxAttempts)
                                 .FirstOrDefaultAsync(cancellationToken);

            if (maxAttempts is null) //=>Unlimited
                return RequestResult<bool>.Success(true);

            int QuizAttemptCount = await _quizAttemptsRepository
                .Get(qa => qa.QuizId == request.QuizId &&
                 qa.StudentId == request.studentId).CountAsync(cancellationToken);

            bool canAttempt = QuizAttemptCount < maxAttempts;

            return RequestResult<bool>.Success(canAttempt);

        }
    }
}
