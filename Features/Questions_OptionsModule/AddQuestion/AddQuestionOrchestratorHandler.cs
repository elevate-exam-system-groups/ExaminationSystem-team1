using ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Commands.CreateOptionsForQuestion;
using ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Commands.CreateQuestion;
using ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Queries.GetQuiz;

namespace ExaminationSystem.Features.Questions_OptionsModule.Command
{
    public class AddQuestionOrchestratorHandler
      : IRequestHandler<AddQuestionOrchestratorCommand, RequestResult<AddQuestionResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public AddQuestionOrchestratorHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<RequestResult<AddQuestionResponse>> Handle(
            AddQuestionOrchestratorCommand request, CancellationToken ct)
        {
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

            await _unitOfWork.SaveChangesAsync();

            return RequestResult<AddQuestionResponse>.Success(
                new AddQuestionResponse(questionId),
                "Question created successfully");
        }

    }
}

