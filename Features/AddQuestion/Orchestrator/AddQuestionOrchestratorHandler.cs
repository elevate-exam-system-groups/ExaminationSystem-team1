using ExaminationSystem.Features.AddQuestion.Commands.CreateOptionsForQuestion;
using ExaminationSystem.Features.AddQuestion.Commands.CreateQuestion;
using ExaminationSystem.Features.AddQuestion.DTOs;
using ExaminationSystem.Features.AddQuestion.Queries.CheckQuizStatus;
using ExaminationSystem.Features.AddQuestion.Queries.GetNextQuestionOrder;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.AddQuestion.Orchestrator
{
    public class AddQuestionOrchestratorHandler
      : IRequestHandler<AddQuestionOrchestratorCommand, RequestResult<AddQuestionResponseDto>>
    {

        private readonly IMediator _mediator;
        public AddQuestionOrchestratorHandler(IMediator mediator)
        => _mediator = mediator;

        public async Task<RequestResult<AddQuestionResponseDto>> Handle(
            AddQuestionOrchestratorCommand request, CancellationToken ct)
        {

            var validationError = ValidateRequest(request);
            if (validationError != null)
                return validationError;

            var quizResult = await _mediator.Send(
                new CheckQuizStatusQuery(request.QuizId), ct);

            if (!quizResult.IsSuccess)
                return RequestResult<AddQuestionResponseDto>.Failure(
                    quizResult.Message,
                    quizResult.requestErrorCode);


            var orderResult = await _mediator.Send(
                new CalculateNextQuestionOrderQuery(request.QuizId), ct);

            if (!orderResult.IsSuccess)
                return RequestResult<AddQuestionResponseDto>.Failure(
                    orderResult.Message,
                    orderResult.requestErrorCode);

            var nextOrder = orderResult.Data.OrderIndex;


            var createQuestionResult = await _mediator.Send(
            new CreateQuestionCommand
            (
                  request.QuizId,
                  request.Text,
                  request.Explanation,
                  nextOrder), ct
            );

            if (!createQuestionResult.IsSuccess)
            {
                return RequestResult<AddQuestionResponseDto>.Failure(
                    createQuestionResult.Message,
                    createQuestionResult.requestErrorCode);
            }

            var questionId = createQuestionResult.Data.QuestionId;

            var createOptionsResult = await _mediator.Send(
                new CreateOptionsForQuestionCommand(questionId, request.Options), ct);

            if (!createOptionsResult.IsSuccess)
            {
                return RequestResult<AddQuestionResponseDto>.Failure(
                    createOptionsResult.Message,
                    createOptionsResult.requestErrorCode);
            }


            return RequestResult<AddQuestionResponseDto>.Success(
                new AddQuestionResponseDto(true),
                "Question created successfully");
        }


        private RequestResult<AddQuestionResponseDto>? ValidateRequest(AddQuestionOrchestratorCommand request)
        {

            RequestResult<AddQuestionResponseDto> Fail(string msg) =>
                RequestResult<AddQuestionResponseDto>.Failure(msg, RequestErrorCode.ValidationError);

            if (string.IsNullOrWhiteSpace(request.Text))
                return Fail("Question text is required");

            if (request.Text.Length is < 3 or > 1000)
                return Fail("Question text must be between 3 and 1000 characters");

            if (request.Options == null || request.Options.Count < 2)
                return Fail("At least 2 options are required");

            var correctCount = request.Options.Count(o => o.IsCorrect);
            if (correctCount != 1)
                return Fail(correctCount == 0 ?
                    "No correct option marked." :
                    $"Found {correctCount} correct options. Exactly one required.");


            foreach (var option in request.Options)
            {
                if (string.IsNullOrWhiteSpace(option.Text))
                    return Fail("Option text cannot be empty");

                if (option.Text.Length > 500)
                    return Fail("Option text cannot exceed 500 characters");
            }


            if (request.Explanation?.Length > 2000)
                return Fail("Explanation cannot exceed 2000 characters");

            return null;
        }


    }
}
