
namespace ExaminationSystem.Features.Attempts.Shared.Queries
{
    public record IsQuizTimerExpiredQuery(Guid attemptId, string studentId)
        : IRequest<RequestResult<bool>>;

    public class IsQuizTimerExpiredQueryValidator
        : AbstractValidator<IsQuizTimerExpiredQuery>
    {
        public IsQuizTimerExpiredQueryValidator()
        {
            RuleFor(x => x.attemptId)
                .NotEmpty().WithMessage("Quiz ID is required");
            RuleFor(x => x.studentId)
                .NotEmpty().WithMessage("Student ID is required");
        }
    }

    public class IsQuizTimerExpiredQueryHandler
        : IRequestHandler<IsQuizTimerExpiredQuery, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<IsQuizTimerExpiredQuery> _validator;
        public IsQuizTimerExpiredQueryHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<IsQuizTimerExpiredQuery> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(IsQuizTimerExpiredQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
             .ValidateRequestAsync<IsQuizTimerExpiredQuery, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;


            bool hasQuizTimerElapsed = await _quizAttemptsRepository
               .Get(qa => qa.Id == request.attemptId &&
                    qa.StudentId == request.studentId &&
                    qa.Status == QuizAttemptStatus.InProgress &&
                    DateTime.UtcNow > qa.DeadLine)
               .AnyAsync();

            return RequestResult<bool>.Success(hasQuizTimerElapsed);
        }
    }
}
