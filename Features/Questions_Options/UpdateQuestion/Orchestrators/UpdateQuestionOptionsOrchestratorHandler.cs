using ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Commands.UpdateQuestionOnly;
using ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Queries.GetQuestionForEdit;

namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Orchestrator
{
    public class UpdateQuestionOptionsOrchestratorHandler
          : IRequestHandler<UpdateQuestionOptionsOrchestrator, RequestResult<UpdateQuestionResponseDto>>
    {

        private readonly IMediator _mediator;
        public UpdateQuestionOptionsOrchestratorHandler(IMediator mediator)
          => _mediator = mediator;

        public async Task<RequestResult<UpdateQuestionResponseDto>> Handle(
            UpdateQuestionOptionsOrchestrator request, CancellationToken ct)
        {

            var validationError = ValidateRequest(request);
            if (validationError != null)
                return validationError;

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



        private RequestResult<UpdateQuestionResponseDto>? ValidateRequest(UpdateQuestionOptionsOrchestrator request)
        {

            RequestResult<UpdateQuestionResponseDto> Fail(string msg)
                => RequestResult<UpdateQuestionResponseDto>.Failure(msg, RequestErrorCode.ValidationError);

            if (request.Id == Guid.Empty)
                return Fail("Question ID is required.");

            if (string.IsNullOrWhiteSpace(request.Text) || request.Text.Length is < 3 or > 1000)
                return Fail("Question text must be between 3 and 1000 characters.");

            if (request.Options == null || request.Options.Count < 2)
                return Fail("At least 2 options are required.");

            var correctCount = request.Options.Count(o => o.IsCorrect);
            if (correctCount != 1)
                return Fail(correctCount == 0 ?
                    "No correct option marked." : $"Found {correctCount} correct options.");

            foreach (var option in request.Options)
            {
                if (string.IsNullOrWhiteSpace(option.Text) || option.Text.Length > 500)
                    return Fail("Option text cannot be empty or exceed 500 characters.");
            }

            if (request.Explanation?.Length > 2000)
                return Fail("Explanation cannot exceed 2000 characters.");

            return null;
        }

    }
}