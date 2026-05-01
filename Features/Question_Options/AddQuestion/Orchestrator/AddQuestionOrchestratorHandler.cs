using ExaminationSystem.Features.QuestionFeatures.AddQuestion.Commands.CreateOptionsForQuestion;
using ExaminationSystem.Features.QuestionFeatures.AddQuestion.Commands.CreateQuestion;
using ExaminationSystem.Features.QuestionFeatures.AddQuestion.DTOs;
using ExaminationSystem.Features.QuestionFeatures.AddQuestion.Queries.CheckQuizStatus;
using ExaminationSystem.Features.QuestionFeatures.AddQuestion.Queries.GetNextQuestionOrder;

namespace ExaminationSystem.Features.QuestionFeatures.AddQuestion.Orchestrator
{
    public class AddQuestionOrchestratorHandler
      : IRequestHandler<AddQuestionOrchestratorCommand, RequestResult<AddQuestionResponseDto>>
    {

        private readonly IMediator _mediator;
        public AddQuestionOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResult<AddQuestionResponseDto>> Handle(
            AddQuestionOrchestratorCommand request, CancellationToken ct)
        {

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


    }
}
