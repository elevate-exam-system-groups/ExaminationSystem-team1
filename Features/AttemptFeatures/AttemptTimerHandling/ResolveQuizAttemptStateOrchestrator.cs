using ExaminationSystem.Features.AttemptFeatures.SharedAttempts.Orchestrators;
using ExaminationSystem.Features.AttemptFeatures.SharedAttempts.Queries;
using ExaminationSystem.Features.AttemptFeatures.SubmitQuizAttempt.Orchestrators.DTOS;

namespace ExaminationSystem.Features.AttemptFeatures.AttemptTimerHandling
{
    public record ResolveQuizAttemptStateOrchestrator(Guid AttemptId)
        : IRequest<RequestResult<SubmitAttemptResultDto?>>;

    public class ResolveQuizAttemptStateOrchestratorValidator : AbstractValidator<ResolveQuizAttemptStateOrchestrator>
    {
        public ResolveQuizAttemptStateOrchestratorValidator()
        {
            RuleFor(x => x.AttemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
        }
    }

    public class ResolveQuizAttemptStateOrchestratorHandler
        : IRequestHandler<ResolveQuizAttemptStateOrchestrator, RequestResult<SubmitAttemptResultDto?>>
    {
        private readonly IMediator _mediator;
        public ResolveQuizAttemptStateOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<RequestResult<SubmitAttemptResultDto?>> Handle(
          ResolveQuizAttemptStateOrchestrator request, CancellationToken cancellationToken)
        {
            var isSubmitted = await _mediator
                .Send(new IsAttemptSubmittedQuery(request.AttemptId), cancellationToken);

            if (isSubmitted.Data)
                return RequestResult<SubmitAttemptResultDto?>
                    .Failure("Quiz attempt is already submitted", RequestErrorCode.Conflict);

            var isTimerExpired = await _mediator
                .Send(new IsQuizTimerExpiredQuery(request.AttemptId), cancellationToken);

            if (!isTimerExpired.Data)
                return RequestResult<SubmitAttemptResultDto?>
                    .Success(null);


            var closeAttemptResult = await _mediator
                .Send(new CompleteQuizAttemptOrchestrator(request.AttemptId, QuizAttemptStatus.TimedOut), cancellationToken);

            if (!closeAttemptResult.IsSuccess)
                return RequestResult<SubmitAttemptResultDto?>
                    .Failure(closeAttemptResult.Message, closeAttemptResult.requestErrorCode);

            return RequestResult<SubmitAttemptResultDto?>
                .Success(closeAttemptResult.Data, "Timer has elapsed", RequestErrorCode.Gone);
        }
    }

}
