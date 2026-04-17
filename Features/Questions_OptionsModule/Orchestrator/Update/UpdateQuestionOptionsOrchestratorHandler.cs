using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;
using ExaminationSystem.Features.Questions_OptionsModule.QuestionForEdit.Command;

namespace ExaminationSystem.Features.Questions_OptionsModule.Orchestrator.Update
{
    public class UpdateQuestionOptionsOrchestratorHandler
          : IRequestHandler<UpdateQuestionOptionsOrchestrator, RequestResult<UpdateQuestionResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateQuestionOptionsOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResult<UpdateQuestionResponse>> Handle(
            UpdateQuestionOptionsOrchestrator request, CancellationToken ct)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                // Update Question text + explanation
                var updateQuestionResult = await _mediator.Send(
                    new UpdateQuestionOnlyCommand(request.Id, request.Text, request.Explanation), ct);

                if (!updateQuestionResult.IsSuccess)
                {
                    await transaction.RollbackAsync(ct);
                    return RequestResult<UpdateQuestionResponse>.Failure(
                        updateQuestionResult.Message, updateQuestionResult.requestErrorCode);
                }

                // Update Options
                var updateOptionsResult = await _mediator.Send(
                    new UpdateOptionsCommand(request.Id, request.Options), ct);

                if (!updateOptionsResult.IsSuccess)
                {
                    await transaction.RollbackAsync(ct);
                    return RequestResult<UpdateQuestionResponse>.Failure(
                        updateOptionsResult.Message, updateOptionsResult.requestErrorCode);
                }

                await transaction.CommitAsync(ct);

                return RequestResult<UpdateQuestionResponse>.Success(
                    new UpdateQuestionResponse(request.Id),
                    "Question and options updated successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                return RequestResult<UpdateQuestionResponse>.Failure(
                    $"An error occurred: {ex.Message}", RequestErrorCode.InternalServerError);
            }
        }
    }
}

