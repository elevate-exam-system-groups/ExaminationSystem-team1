using ExaminationSystem.Features.AttemptFeatures.SharedAttempts.Commands;
using ExaminationSystem.Features.AttemptFeatures.SubmitQuizAttempt.Commands;
using ExaminationSystem.Features.AttemptFeatures.SubmitQuizAttempt.Orchestrators.DTOS;
using ExaminationSystem.Features.AttemptFeatures.SubmitQuizAttempt.Queries.GetAttemptResult;


namespace ExaminationSystem.Features.AttemptFeatures.SharedAttempts.Orchestrators
{
    public record CompleteQuizAttemptOrchestrator(Guid AttemptId, QuizAttemptStatus Status)
        : IRequest<RequestResult<SubmitAttemptResultDto>>;

    public class CompleteQuizAttemptOrchestratorValidator
        : AbstractValidator<CompleteQuizAttemptOrchestrator>
    {
        public CompleteQuizAttemptOrchestratorValidator()
        {
            RuleFor(x => x.AttemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid attempt status");
        }
    }

    public class CompleteQuizAttemptOrchestratorHandler
        : IRequestHandler<CompleteQuizAttemptOrchestrator, RequestResult<SubmitAttemptResultDto>>
    {
        private readonly IMediator _mediator;
        private readonly IValidator<CompleteQuizAttemptOrchestrator> _validator;

        public CompleteQuizAttemptOrchestratorHandler(IMediator mediator, IValidator<CompleteQuizAttemptOrchestrator> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        public async Task<RequestResult<SubmitAttemptResultDto>> Handle(CompleteQuizAttemptOrchestrator request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
                .ValidateRequestAsync<CompleteQuizAttemptOrchestrator, SubmitAttemptResultDto>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var updateResult = await _mediator
               .Send(new UpdateQuizAttemptStatusCommand(request.AttemptId, request.Status), cancellationToken);

            if (!updateResult.Data)
                return RequestResult<SubmitAttemptResultDto>
                    .Failure(updateResult.Message, updateResult.requestErrorCode);

            var result = await _mediator
                .Send(new GetAttemptResultQuery(request.AttemptId), cancellationToken);

            if (!result.IsSuccess)
                return RequestResult<SubmitAttemptResultDto>
                    .Failure(result.Message, result.requestErrorCode);
            var setResult = await _mediator
                .Send(new SetAttemptResultCommand(request.AttemptId, result.Data.score, result.Data.isPassed), cancellationToken);

            if (!setResult.Data)
                return RequestResult<SubmitAttemptResultDto>
                    .Failure("Failed to set attempt result", RequestErrorCode.InternalServerError);

            return RequestResult<SubmitAttemptResultDto>.Success(
                new SubmitAttemptResultDto
                (
                    result.Data.score,
                    result.Data.isPassed,
                    request.Status
                ));
        }
    }
}
