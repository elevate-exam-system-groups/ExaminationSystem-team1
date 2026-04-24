using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.UnpublishQuiz.Queries
{
    public record CheckQuizHasCurrentAttemptsQueryRequest(Guid quizId) : IRequest<RequestResult<bool>>;

    public class CheckQuizHasCurrentAttemptsQueryRequestValidator : AbstractValidator<CheckQuizHasCurrentAttemptsQueryRequest>
    {
        public CheckQuizHasCurrentAttemptsQueryRequestValidator()
        {
            RuleFor(r => r.quizId)
                .NotEmpty()
                .WithMessage("Quiz Id is required.");
        }
    }

    public class CheckQuizHasCurrentAttemptsQueryRequestHandler
        : IRequestHandler<CheckQuizHasCurrentAttemptsQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptRepository;
        private readonly IValidator<CheckQuizHasCurrentAttemptsQueryRequest> _validator;

        public CheckQuizHasCurrentAttemptsQueryRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptRepository, IValidator<CheckQuizHasCurrentAttemptsQueryRequest> validator)
        {
            _quizAttemptRepository = quizAttemptRepository;
            _validator = validator;
        }

        public async Task<RequestResult<bool>> Handle(CheckQuizHasCurrentAttemptsQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
             .ValidateRequestAsync<CheckQuizHasCurrentAttemptsQueryRequest, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var HasAttempts = await _quizAttemptRepository
                .Get(qa => qa.QuizId == request.quizId && qa.Status == QuizAttemptStatus.InProgress)
                .AnyAsync();

            if (HasAttempts)
            {
                return RequestResult<bool>
                    .Failure("Quiz has enrollments and cannot be unpublished.", RequestErrorCode.QuizHasEnrollments);
            }

            return RequestResult<bool>.Success(HasAttempts);
        }
    }


}
