using ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Commands.UpdateOptions;
using ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Commands.UpdateQuestionOnly;
using ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Dtos;
using ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Queries.QuestionUpdateStatus;

namespace ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Orchestrators
{
    public class UpdateQuestionOptionsOrchestratorHandler
          : IRequestHandler<UpdateQuestionOptionsOrchestrator, RequestResult<UpdateQuestionResponseDto>>
    {

        private readonly IMediator _mediator;
        public UpdateQuestionOptionsOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResult<UpdateQuestionResponseDto>> Handle(
            UpdateQuestionOptionsOrchestrator request, CancellationToken ct)
        {

            var StatusResult = await _mediator.Send(new GetQuestionUpdateStatusQuery(request.Id), ct);

            if (!StatusResult.IsSuccess)
                return RequestResult<UpdateQuestionResponseDto>.Failure(
                    StatusResult.Message,
                    StatusResult.requestErrorCode);

            if (StatusResult.Data.QuizStatus == QuizStatus.Published)
                return RequestResult<UpdateQuestionResponseDto>.Failure(
                    "Cannot update question in a published quiz. Unpublish first.",
                    RequestErrorCode.Conflict);


            var updateQuestionResult = await _mediator.Send(
                new UpdateQuestionOnlyCommand(request.Id, request.Text, request.Explanation), ct);

            if (!updateQuestionResult.IsSuccess)
            {
                return RequestResult<UpdateQuestionResponseDto>.Failure(
                    updateQuestionResult.Message, updateQuestionResult.requestErrorCode);
            }

            var updateOptionsResult = await _mediator.Send(
                new UpdateOptionsCommand(request.Id, request.Options), ct);

            if (!updateOptionsResult.IsSuccess)
            {
                return RequestResult<UpdateQuestionResponseDto>.Failure(
                    updateOptionsResult.Message, updateOptionsResult.requestErrorCode);
            }


            return RequestResult<UpdateQuestionResponseDto>.Success(
                new UpdateQuestionResponseDto(true),
                "Question and options updated successfully.");
        }

    }
}