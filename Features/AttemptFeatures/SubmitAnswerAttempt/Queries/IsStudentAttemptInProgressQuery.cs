using ExaminationSystem.Features.Common.FeatureExtensions;

namespace ExaminationSystem.Features.AttemptFeatures.SubmitAnswerAttempt.Queries
{
    public record IsStudentAttemptInProgressQuery(Guid attemptId, string studentId)
        : IRequest<RequestResult<bool>>;

    public class IsStudentAttemptInProgressQueryValidator : AbstractValidator<IsStudentAttemptInProgressQuery>
    {
        public IsStudentAttemptInProgressQueryValidator()
        {
            RuleFor(x => x.attemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
            RuleFor(x => x.studentId)
                .NotEmpty().WithMessage("Student ID is required");
        }
    }

    public class IsStudentAttemptInProgressQueryHandler
        : IRequestHandler<IsStudentAttemptInProgressQuery, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<IsStudentAttemptInProgressQuery> _validator;
        public IsStudentAttemptInProgressQueryHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<IsStudentAttemptInProgressQuery> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(IsStudentAttemptInProgressQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
                .ValidateRequestAsync<IsStudentAttemptInProgressQuery, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;


            var isInProgressAttempt = await _quizAttemptsRepository
               .Get(qa => qa.Id == request.attemptId &&
                    qa.StudentId == request.studentId &&
                    qa.Status == QuizAttemptStatus.InProgress)
               .AnyAsync(cancellationToken);

            if (!isInProgressAttempt)
            {
                return RequestResult<bool>
                .Failure("Attempt is already submitted or expired", RequestErrorCode.Conflict);
            }

            return RequestResult<bool>.Success(isInProgressAttempt);
        }
    }
}