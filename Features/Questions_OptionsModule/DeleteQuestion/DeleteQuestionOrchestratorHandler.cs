namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion
{
    public class DeleteQuestionOrchestratorHandler
        : IRequestHandler<DeleteQuestionOrchestrator, RequestResult<DeleteResponse>>
    {

        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteQuestionOrchestratorHandler(
            IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResult<DeleteResponse>> Handle(
            DeleteQuestionOrchestrator request, CancellationToken ct)
        {
            // Get question info 
            var questionInfo = await _mediator.Send(
                 new GetQuestionInfoQuery(request.Id), ct);

            if (!questionInfo.IsSuccess)
                return RequestResult<DeleteResponse>.Failure(
                    questionInfo.Message,
                    questionInfo.requestErrorCode);

            var result = questionInfo.Data;

            // Cannot delete from published quiz
            if (result.QuizStatus == QuizStatus.Published)
            {
                return RequestResult<DeleteResponse>.Failure(
                    "Cannot delete question from a published quiz. Unpublish quiz first.",
                    RequestErrorCode.Conflict);
            }

            // Check for active attempts
            var checkAttemptsResult = await _mediator.Send(
                new CheckActiveAttemptsQuery(result.QuizId), ct);
            if (!checkAttemptsResult.IsSuccess)
                return RequestResult<DeleteResponse>.Failure(
                      "Cannot delete question while there are active quiz attempts.",
                    RequestErrorCode.Conflict);



            // Proceed with deletion
            await using var transaction = await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                // Delete options
                var deleteOptionsResult = await _mediator.Send(
                    new DeleteOptionsCommand(request.Id), ct);

                if (!deleteOptionsResult.IsSuccess)
                {
                    await transaction.RollbackAsync(ct);
                    return RequestResult<DeleteResponse>.Failure(
                        deleteOptionsResult.Message,
                        deleteOptionsResult.requestErrorCode);
                }

                // Delete question
                var deleteQuestionResult = await _mediator.Send(
                    new DeleteQuestionOnlyCommand(request.Id), ct);

                if (!deleteQuestionResult.IsSuccess)
                {
                    await transaction.RollbackAsync(ct);
                    return RequestResult<DeleteResponse>.Failure(
                        deleteQuestionResult.Message,
                        deleteQuestionResult.requestErrorCode);
                }

                await transaction.CommitAsync(ct);
                return RequestResult<DeleteResponse>.Success(
                    new DeleteResponse(true),
                    "Question and all options deleted successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                return RequestResult<DeleteResponse>.Failure(
                    $"Delete failed: {ex.Message}",
                    RequestErrorCode.InternalServerError);
            }
        }
    }
}
