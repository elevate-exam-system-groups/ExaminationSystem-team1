using ExaminationSystem.Features.Common.FeatureExtensions;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Common.AttemptRequests.Queries
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
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<CheckStudentAttemptOwnershipQuery> _validator;
        public CheckStudentAttemptOwnershipQueryHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<CheckStudentAttemptOwnershipQuery> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(CheckStudentAttemptOwnershipQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
               .ValidateRequestAsync<CheckStudentAttemptOwnershipQuery, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

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
