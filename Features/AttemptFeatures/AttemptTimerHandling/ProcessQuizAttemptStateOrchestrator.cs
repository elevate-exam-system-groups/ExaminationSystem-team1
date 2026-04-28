using ExaminationSystem.Features.Common.AttemptRequests.Orchestrators;

namespace ExaminationSystem.Features.AttemptFeatures.AttemptTimerHandling
{
    public record ProcessQuizAttemptStateOrchestrator(Guid AttemptId)
        : IRequest<RequestResult<bool>>;

    public class ProcessQuizAttemptStateOrchestratorValidator : AbstractValidator<ProcessQuizAttemptStateOrchestrator>
    {
        public ProcessQuizAttemptStateOrchestratorValidator()
        {
            RuleFor(x => x.AttemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
        }
    }

    public class ProcessQuizAttemptStateOrchestratorHandler
        : IRequestHandler<ProcessQuizAttemptStateOrchestrator, RequestResult<bool>>
    {
        private readonly IMediator _mediator;
        public ProcessQuizAttemptStateOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<RequestResult<bool>> Handle(ProcessQuizAttemptStateOrchestrator request, CancellationToken cancellationToken)
        {
            var isSubmitted = await _mediator
                .Send(new IsAttemptSubmittedQuery(request.AttemptId), cancellationToken);

            if (isSubmitted.Data)
            {
                return RequestResult<bool>
                    .Success(true, "Quiz attempt is already submitted");
            }

            var isTimerExpired = await _mediator
                .Send(new IsQuizTimerExpiredQuery(request.AttemptId), cancellationToken);

            if (!isTimerExpired.Data)
            {
                return RequestResult<bool>
                    .Success(isTimerExpired.Data, "Quiz timer has not expired yet");
            }

            var closeAttemptResult = await _mediator
                .Send(new CompleteQuizAttemptOrchestrator(request.AttemptId, QuizAttemptStatus.TimedOut), cancellationToken);

            if (!closeAttemptResult.IsSuccess)
            {
                return RequestResult<bool>
                    .Failure(closeAttemptResult.Message, closeAttemptResult.requestErrorCode);
            }

            return RequestResult<bool>.Success(true);
        }
    }

}
