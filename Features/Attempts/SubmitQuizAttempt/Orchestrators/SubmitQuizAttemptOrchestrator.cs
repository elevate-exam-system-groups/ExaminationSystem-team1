using ExaminationSystem.Features.Attempts.Shared.Queries;
using ExaminationSystem.Features.Attempts.SubmitQuizAttempt.Orchestrators.DTOS;
using ExaminationSystem.Features.Attempts.SubmitQuizAttempt.Orchestrators.ManageSubmission;


namespace ExaminationSystem.Features.Attempts.SubmitQuizAttempt.Orchestrators
{
    public record SubmitQuizAttemptOrchestrator(string StudentId, Guid attemptId)
        : IRequest<RequestResult<SubmitAttemptResultDto>>;

    public class SubmitQuizAttemptOrchestratorValidator : AbstractValidator<SubmitQuizAttemptOrchestrator>
    {
        public SubmitQuizAttemptOrchestratorValidator()
        {
            RuleFor(r => r.StudentId)
                .NotEmpty()
                .WithMessage("Student Id is required.");

            RuleFor(r => r.attemptId)
                .NotEmpty()
                .WithMessage("Attempt Id is required.");
        }
    }

    public class SubmitQuizAttemptOrchestratorHandler
        : IRequestHandler<SubmitQuizAttemptOrchestrator, RequestResult<SubmitAttemptResultDto>>
    {
        private readonly IMediator _mediator;
        private readonly IValidator<SubmitQuizAttemptOrchestrator> _validator;
        public SubmitQuizAttemptOrchestratorHandler(IMediator mediator, IValidator<SubmitQuizAttemptOrchestrator> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }
        public async Task<RequestResult<SubmitAttemptResultDto>> Handle(SubmitQuizAttemptOrchestrator request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
                .ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string
                    .Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<SubmitAttemptResultDto>
                    .Failure(validationErrors, RequestErrorCode.ValidationError);
            }

            var doesStudentOwnTheAttempt = await _mediator
                .Send(new CheckStudentAttemptOwnershipQuery(request.attemptId, request.StudentId), cancellationToken);

            if (!doesStudentOwnTheAttempt.Data)
            {
                return RequestResult<SubmitAttemptResultDto>
                    .Failure(doesStudentOwnTheAttempt.Message!, doesStudentOwnTheAttempt.requestErrorCode);
            }

            var isAttemptSubmitted = await _mediator
                .Send(new IsAttemptSubmittedQuery(request.attemptId), cancellationToken);

            if (isAttemptSubmitted.Data)
            {
                return RequestResult<SubmitAttemptResultDto>
                      .Failure("Attempt is Already Submitted", RequestErrorCode.Conflict);
            }

            var isQuizTimerExpired = await _mediator
                .Send(new IsQuizTimerExpiredQuery(request.attemptId, request.StudentId), cancellationToken);

            if (isQuizTimerExpired.Data)
            {
                var Result = await _mediator
                    .Send(new FinalizeAttemptSubmissionOrchestrator(request.attemptId, QuizAttemptStatus.TimedOut), cancellationToken);

                return Result.IsSuccess
                    ? RequestResult<SubmitAttemptResultDto>
                    .Success(Result.Data!, "Quiz timer has expired", RequestErrorCode.Gone)
                    : RequestResult<SubmitAttemptResultDto>
                    .Failure(Result.Message!, Result.requestErrorCode);
            }

            var FinalResult = await _mediator
                        .Send(new FinalizeAttemptSubmissionOrchestrator(request.attemptId, QuizAttemptStatus.Submitted), cancellationToken);

            return FinalResult.IsSuccess
                ? RequestResult<SubmitAttemptResultDto>
                .Success(FinalResult.Data!)
                : RequestResult<SubmitAttemptResultDto>
                .Failure(FinalResult.Message!, FinalResult.requestErrorCode);
        }
    }
}
