using ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Commands.CreateOptionsForQuestion;
using ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Commands.CreateQuestion;
using ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Queries.GetQuiz;

namespace ExaminationSystem.Features.Questions_OptionsModule.Command
{
    public class AddQuestionOrchestratorHandler
      : IRequestHandler<AddQuestionOrchestratorCommand, RequestResult<AddQuestionResponse>>
    {

        private readonly IMediator _mediator;
        public AddQuestionOrchestratorHandler(IMediator mediator)
        => _mediator = mediator;

        public async Task<RequestResult<AddQuestionResponse>> Handle(
            AddQuestionOrchestratorCommand request, CancellationToken ct)
        {

            var validationError = ValidateRequest(request);
            if (validationError != null)
                return validationError;

            var quizResult = await _mediator.Send(new GetQuizByIdQuery(request.QuizId), ct);

            if (!quizResult.IsSuccess)
                return RequestResult<AddQuestionResponse>.Failure(
                    quizResult.Message,
                    quizResult.requestErrorCode);

            var quiz = quizResult.Data;

            if (quiz.Status == QuizStatus.Published)
                return RequestResult<AddQuestionResponse>.Failure(
                    "Cannot add question to published quiz",
                    RequestErrorCode.Conflict);

            var orderResult = await _mediator.Send(
                new GetNextQuestionOrderQuery(request.QuizId), ct);

            if (!orderResult.IsSuccess)
                return RequestResult<AddQuestionResponse>.Failure(
                    orderResult.Message,
                    orderResult.requestErrorCode);

            var nextOrder = orderResult.Data.OrderIndex;


            var createQuestionResult = await _mediator.Send(new CreateQuestionCommand
            (
                  request.QuizId,
                  request.Text,
                  request.Explanation,
                  nextOrder), ct
            );

            if (!createQuestionResult.IsSuccess)
            {
                return RequestResult<AddQuestionResponse>.Failure(
                    createQuestionResult.Message,
                    createQuestionResult.requestErrorCode);
            }
            var questionId = createQuestionResult.Data.QuestionId;

            var optionsCommand = new CreateOptionsForQuestionCommand(questionId, request.Options);
            var createOptionsResult = await _mediator.Send(optionsCommand, ct);

            if (!createOptionsResult.IsSuccess)
            {
                return RequestResult<AddQuestionResponse>.Failure(
                    createOptionsResult.Message,
                    createOptionsResult.requestErrorCode);
            }


            return RequestResult<AddQuestionResponse>.Success(
                new AddQuestionResponse(questionId),
                "Question created successfully");
        }

        private RequestResult<AddQuestionResponse>? ValidateRequest(AddQuestionOrchestratorCommand request)
        {
            // 1. Validate Question Text
            if (string.IsNullOrWhiteSpace(request.Text))
                return RequestResult<AddQuestionResponse>.Failure(
                    "Question text is required",
                    RequestErrorCode.ValidationError);

            if (request.Text.Length < 3 || request.Text.Length > 1000)
                return RequestResult<AddQuestionResponse>.Failure(
                    "Question text must be between 3 and 1000 characters",
                    RequestErrorCode.ValidationError);

            // 2. Validate Options
            if (request.Options == null || request.Options.Count < 2)
                return RequestResult<AddQuestionResponse>.Failure(
                    "At least 2 options are required",
                    RequestErrorCode.ValidationError);

            // 3. Validate Exactly One Correct Answer
            var correctOptionsCount = request.Options.Count(o => o.IsCorrect);

            if (correctOptionsCount == 0)
                return RequestResult<AddQuestionResponse>.Failure(
                    "Exactly one correct option is required. No correct option marked.",
                    RequestErrorCode.ValidationError);

            if (correctOptionsCount > 1)
                return RequestResult<AddQuestionResponse>.Failure(
                    $"Exactly one correct option is required. Found {correctOptionsCount} correct options.",
                    RequestErrorCode.ValidationError);

            // 4. Validate Option Text
            foreach (var option in request.Options)
            {
                if (string.IsNullOrWhiteSpace(option.Text))
                    return RequestResult<AddQuestionResponse>.Failure(
                        "Option text cannot be empty",
                        RequestErrorCode.ValidationError);

                if (option.Text.Length > 500)
                    return RequestResult<AddQuestionResponse>.Failure(
                        "Option text cannot exceed 500 characters",
                        RequestErrorCode.ValidationError);
            }

            // 5. Validate Explanation (optional but with length limit)
            if (!string.IsNullOrWhiteSpace(request.Explanation) && request.Explanation.Length > 2000)
                return RequestResult<AddQuestionResponse>.Failure(
                    "Explanation cannot exceed 2000 characters",
                    RequestErrorCode.ValidationError);

            return null; 
        }
    }
}
