using ExaminationSystem.Features.Common.QuizRequests.Queries;
using ExaminationSystem.Features.Common.Request;


namespace ExaminationSystem.Features.Common.AttemptRequests.Queries
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
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IMediator _mediator;
        private readonly IValidator<CanStudentAttemptQuizQuery> _validator;
        public CanStudentAttemptQuizQueryHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<CanStudentAttemptQuizQuery> validator, IMediator mediator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
            _mediator = mediator;
        }
        public async Task<RequestResult<bool>> Handle(CanStudentAttemptQuizQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
             .ValidateRequestAsync<CanStudentAttemptQuizQuery, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var maxAttemptsResult = await _mediator
                .Send(new GetQuizMaxAttemptsQuery(request.QuizId), cancellationToken);

            if (maxAttemptsResult.Data is null) // =>unlimited
                return RequestResult<bool>.Success(true);

            int QuizAttemptCount = await _quizAttemptsRepository
                .Get(qa => qa.QuizId == request.QuizId &&
                 qa.StudentId == request.studentId).CountAsync(cancellationToken);

            bool canAttempt = QuizAttemptCount < maxAttemptsResult.Data;

            return RequestResult<bool>.Success(canAttempt);

        }
    }
}
