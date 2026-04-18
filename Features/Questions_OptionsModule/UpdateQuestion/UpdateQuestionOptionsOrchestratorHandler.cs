namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion
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
            // Update Question text + explanation
            var updateQuestionResult = await _mediator.Send(
                new UpdateQuestionOnlyCommand(request.Id, request.Text, request.Explanation), ct);

            if (!updateQuestionResult.IsSuccess)
            {
                return RequestResult<UpdateQuestionResponse>.Failure(
                    updateQuestionResult.Message, updateQuestionResult.requestErrorCode);
            }

            // Update Options
            var updateOptionsResult = await _mediator.Send(
                new UpdateOptionsCommand(request.Id, request.Options), ct);

            if (!updateOptionsResult.IsSuccess)
            {
                return RequestResult<UpdateQuestionResponse>.Failure(
                    updateOptionsResult.Message, updateOptionsResult.requestErrorCode);
            }
            await _unitOfWork.SaveChangesAsync();

            return RequestResult<UpdateQuestionResponse>.Success(
                new UpdateQuestionResponse(request.Id),
                "Question and options updated successfully.");
        }


    }
}

