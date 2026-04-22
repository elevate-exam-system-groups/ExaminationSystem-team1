using ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Commands.UpdateQuestionOnly;

namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion
{
    public class UpdateQuestionOptionsOrchestratorHandler
          : IRequestHandler<UpdateQuestionOptionsOrchestrator, RequestResult<UpdateQuestionResponse>>
    {
        
        private readonly IMediator _mediator;
        public UpdateQuestionOptionsOrchestratorHandler(IMediator mediator)
          => _mediator = mediator;

        public async Task<RequestResult<UpdateQuestionResponse>> Handle(
            UpdateQuestionOptionsOrchestrator request, CancellationToken ct)
        {

            var validationError = ValidateRequest(request);
            if (validationError != null)
                return validationError;

            var updateQuestionResult = await _mediator.Send(
                new UpdateQuestionOnlyCommand(request.Id, request.Text, request.Explanation), ct);

            if (!updateQuestionResult.IsSuccess)
            {
                return RequestResult<UpdateQuestionResponse>.Failure(
                    updateQuestionResult.Message, updateQuestionResult.requestErrorCode);
            }

            var updateOptionsResult = await _mediator.Send(
                new UpdateOptionsCommand(request.Id, request.Options), ct);

            if (!updateOptionsResult.IsSuccess)
            {
                return RequestResult<UpdateQuestionResponse>.Failure(
                    updateOptionsResult.Message, updateOptionsResult.requestErrorCode);
            }


            return RequestResult<UpdateQuestionResponse>.Success(
                new UpdateQuestionResponse(request.Id),
                "Question and options updated successfully.");
        }

        private RequestResult<UpdateQuestionResponse>? ValidateRequest(UpdateQuestionOptionsOrchestrator request)
        {
            // 1. Validate Question ID
            if (request.Id == Guid.Empty)
                return RequestResult<UpdateQuestionResponse>.Failure(
                    "Question ID is required",
                    RequestErrorCode.ValidationError);

            // 2. Validate Question Text
            if (string.IsNullOrWhiteSpace(request.Text))
                return RequestResult<UpdateQuestionResponse>.Failure(
                    "Question text is required",
                    RequestErrorCode.ValidationError);

            if (request.Text.Length < 3 || request.Text.Length > 1000)
                return RequestResult<UpdateQuestionResponse>.Failure(
                    "Question text must be between 3 and 1000 characters",
                    RequestErrorCode.ValidationError);

            // 3. Validate Options
            if (request.Options == null || request.Options.Count < 2)
                return RequestResult<UpdateQuestionResponse>.Failure(
                    "At least 2 options are required",
                    RequestErrorCode.ValidationError);

            // 4. Validate Exactly One Correct Answer
            var correctOptionsCount = request.Options.Count(o => o.IsCorrect);

            if (correctOptionsCount == 0)
                return RequestResult<UpdateQuestionResponse>.Failure(
                    "Exactly one correct option is required. No correct option marked.",
                    RequestErrorCode.ValidationError);

            if (correctOptionsCount > 1)
                return RequestResult<UpdateQuestionResponse>.Failure(
                    $"Exactly one correct option is required. Found {correctOptionsCount} correct options.",
                    RequestErrorCode.ValidationError);

            // 5. Validate Option Text
            foreach (var option in request.Options)
            {
                if (string.IsNullOrWhiteSpace(option.Text))
                    return RequestResult<UpdateQuestionResponse>.Failure(
                        "Option text cannot be empty",
                        RequestErrorCode.ValidationError);

                if (option.Text.Length > 500)
                    return RequestResult<UpdateQuestionResponse>.Failure(
                        "Option text cannot exceed 500 characters",
                        RequestErrorCode.ValidationError);
            }

            // 6. Validate Explanation (optional but with length limit)
            if (!string.IsNullOrWhiteSpace(request.Explanation) && request.Explanation.Length > 2000)
                return RequestResult<UpdateQuestionResponse>.Failure(
                    "Explanation cannot exceed 2000 characters",
                    RequestErrorCode.ValidationError);

            return null; 
        }
    }
}