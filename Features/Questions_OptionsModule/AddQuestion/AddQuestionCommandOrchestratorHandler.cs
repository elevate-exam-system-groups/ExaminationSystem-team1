namespace ExaminationSystem.Features.Questions_OptionsModule.Command
{
    public class AddQuestionCommandOrchestratorHandler
      : IRequestHandler<AddQuestionOrchestratorCommand, RequestResult<AddQuestionResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public AddQuestionCommandOrchestratorHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<RequestResult<AddQuestionResponse>> Handle(
            AddQuestionOrchestratorCommand request, CancellationToken ct)
        {
            //  Validate Quiz exists and returned
            var quizResult = await _mediator.Send(new GetQuizQuery(request.QuizId), ct);

            if (!quizResult.IsSuccess)
                return RequestResult<AddQuestionResponse>.Failure(
                    quizResult.Message,
                    quizResult.requestErrorCode);

            var quiz = quizResult.Data;

            // Validate Quiz is not Published
            if (quiz.Status == QuizStatus.Published)
                return RequestResult<AddQuestionResponse>.Failure(
                    "Cannot add question to published quiz",
                    RequestErrorCode.Conflict);

            // Get Next OrderIndex
            var orderResult = await _mediator.Send(
                new GetNextQuestionOrderQuery(request.QuizId), ct);

            if (!orderResult.IsSuccess)
                return RequestResult<AddQuestionResponse>.Failure(
                    orderResult.Message,
                    orderResult.requestErrorCode);

            var nextOrder = orderResult.Data.OrderIndex;

            await using var transaction = await _unitOfWork.BeginTransactionAsync(ct);

            try
            {
                // Create Question
                var createQuestionResult = await _mediator.Send(
                    new CreateQuestionCommand(
                        request.QuizId,
                        request.Text,
                        request.Explanation,
                        nextOrder),
                    ct);

                if (!createQuestionResult.IsSuccess)
                {
                    await transaction.RollbackAsync(ct);
                    return RequestResult<AddQuestionResponse>.Failure(
                        createQuestionResult.Message,
                        createQuestionResult.requestErrorCode);
                }

                var questionId = createQuestionResult.Data.QuestionId;

                // Create Options
                var optionsCommand = new CreateOptionsForQuestionCommand(
                    questionId,
                    request.Options.Select(o => new OptionDto
                    (
                        o.Text, 
                        o.IsCorrect
                    )).ToList()
                );

                var createOptionsResult = await _mediator.Send(optionsCommand, ct);

                if (!createOptionsResult.IsSuccess)
                {
                    await transaction.RollbackAsync(ct);
                    return RequestResult<AddQuestionResponse>.Failure(
                        createOptionsResult.Message,
                        createOptionsResult.requestErrorCode);
                }

                //  Save & Commit
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync(ct);

                return RequestResult<AddQuestionResponse>.Success(
                    new AddQuestionResponse(questionId),
                    "Question created successfully");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }
    }

}