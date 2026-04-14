namespace ExaminationSystem.Features.Questions_OptionsModule.Orchestrator.AddQuestion
{
    public class AddQuestionOrchestratorHandler 
        : IRequestHandler<AddQuestionOrchestrator, RequestResult<AddQuestionResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public AddQuestionOrchestratorHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<RequestResult<AddQuestionResponse>> Handle
            (AddQuestionOrchestrator request, CancellationToken cancellationToken)
        {

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // 1. Check if Quiz exists and get its status
                var quizStatusResult = await _mediator.Send(new GetQuizStatusQuery(request.QuizId), cancellationToken);

                if (!quizStatusResult.IsSuccess)
                    return RequestResult<AddQuestionResponse>.Failure(quizStatusResult.Message);

                // .Quiz Published Check
                var isPublishedResult = await _mediator.Send(new IsQuizPublishedQuery(request.QuizId), cancellationToken);

                if (isPublishedResult.IsSuccess && isPublishedResult.Data)
                    return RequestResult<AddQuestionResponse>.Failure
                        ("Cannot modify published quiz.", RequestErrorCode.Conflict);

                // 2. Get next order index
                var orderResult = await _mediator.Send(new GetNextOrderIndexQuery(request.QuizId), cancellationToken);
                if (!orderResult.IsSuccess)
                    return RequestResult<AddQuestionResponse>.Failure("Failed to get next order index");

                // 3. Create Question
                var createQuestionResult = await _mediator.Send(new CreateQuestionCommand
                {
                    QuizId = request.QuizId,
                    Text = request.Text,
                    Explanation = request.Explanation,
                    OrderIndex = orderResult.Data.NextOrderIndex
                }, cancellationToken);

                if (!createQuestionResult.IsSuccess)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return RequestResult<AddQuestionResponse>.Failure(createQuestionResult.Message);
                }

                // 4. Create Options
                var createOptionsResult = await _mediator.Send(new CreateOptionsCommand
                (
                    createQuestionResult.Data.QuestionId,
                    request.Options
                ), cancellationToken);

                if (!createOptionsResult.IsSuccess)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return RequestResult<AddQuestionResponse>.Failure(createOptionsResult.Message);
                }

                // All Success
                await transaction.CommitAsync(cancellationToken);

                return RequestResult<AddQuestionResponse>.Success(
                    new AddQuestionResponse(createQuestionResult.Data.QuestionId),
                    "Question created successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return RequestResult<AddQuestionResponse>.Failure
                    ($"An error occurred: {ex.Message}", RequestErrorCode.ValidationError);
            }
        }
    }
}