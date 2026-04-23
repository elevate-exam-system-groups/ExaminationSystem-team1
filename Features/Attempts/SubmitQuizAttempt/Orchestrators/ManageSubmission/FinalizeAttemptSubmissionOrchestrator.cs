using ExaminationSystem.Features.Attempts.Shared.Commands;
using ExaminationSystem.Features.Attempts.SubmitQuizAttempt.Orchestrators.DTOS;
using ExaminationSystem.Features.Attempts.SubmitQuizAttempt.Requests.Commands;
using ExaminationSystem.Features.Attempts.SubmitQuizAttempt.Requests.Queries.GetAttemptResult;


namespace ExaminationSystem.Features.Attempts.SubmitQuizAttempt.Orchestrators.ManageSubmission
{
    public record FinalizeAttemptSubmissionOrchestrator(Guid AttemptId, QuizAttemptStatus Status)
        : IRequest<RequestResult<SubmitAttemptResultDto>>;

    public class FinalizeAttemptSubmissionOrchestratorHandler
        : IRequestHandler<FinalizeAttemptSubmissionOrchestrator, RequestResult<SubmitAttemptResultDto>>
    {
        private readonly IMediator _mediator;

        public FinalizeAttemptSubmissionOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResult<SubmitAttemptResultDto>> Handle(FinalizeAttemptSubmissionOrchestrator request, CancellationToken cancellationToken)
        {
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
