using ExaminationSystem.Features.Questions_OptionsModule.Common.Command.Request;
using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.Orchestrator.Delete
{
    public class DeleteQuestionOrchestratorHandler
        : IRequestHandler<DeleteQuestionOrchestrator, RequestResult<DeleteQuestionResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteQuestionOrchestratorHandler(
            IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResult<DeleteQuestionResponse>> Handle(
            DeleteQuestionOrchestrator request, CancellationToken ct)
        {
            // 1️⃣ Validate using the validation command
            var validationResult = await _mediator.Send(
                new ValidateQuestionDeletionCommand(request.Id), ct);

            if (!validationResult.IsSuccess)
            {
                return RequestResult<DeleteQuestionResponse>.Failure(
                    validationResult.Message,
                    validationResult.requestErrorCode);
            }

            // 2️⃣ Proceed with deletion
            await using var transaction = await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                // Delete options
                var deleteOptionsResult = await _mediator.Send(
                    new DeleteOptionsCommand(request.Id), ct);

                if (!deleteOptionsResult.IsSuccess)
                {
                    await transaction.RollbackAsync(ct);
                    return RequestResult<DeleteQuestionResponse>.Failure(
                        deleteOptionsResult.Message,
                        deleteOptionsResult.requestErrorCode);
                }

                // Delete question
                var deleteQuestionResult = await _mediator.Send(
                    new DeleteQuestionOnlyCommand(request.Id), ct);

                if (!deleteQuestionResult.IsSuccess)
                {
                    await transaction.RollbackAsync(ct);
                    return RequestResult<DeleteQuestionResponse>.Failure(
                        deleteQuestionResult.Message,
                        deleteQuestionResult.requestErrorCode);
                }

                await transaction.CommitAsync(ct);
                return RequestResult<DeleteQuestionResponse>.Success(
                    new DeleteQuestionResponse(true),
                    "Question and all options deleted successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                return RequestResult<DeleteQuestionResponse>.Failure(
                    $"Delete failed: {ex.Message}",
                    RequestErrorCode.InternalServerError);
            }
        }
    }
}
