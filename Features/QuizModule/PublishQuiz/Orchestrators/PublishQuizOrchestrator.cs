using ExaminationSystem.Features.QuizModule.PublishQuiz.Requests;
using ExaminationSystem.Features.QuizModule.Shared;


namespace ExaminationSystem.Features.QuizModule.PublishQuiz.Orchestrators
{
    public record PublishQuizOrchestrator(Guid quizId) : IRequest<RequestResult<bool>>;

    public class PublishQuizOrchestratorValidator : AbstractValidator<PublishQuizOrchestrator>
    {
        public PublishQuizOrchestratorValidator()
        {
            RuleFor(r => r.quizId)
                .NotEmpty()
                .WithMessage("Quiz Id is required.");
        }
    }

    public class PublishQuizOrchestratorHandler : IRequestHandler<PublishQuizOrchestrator, RequestResult<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IValidator<PublishQuizOrchestrator> _validator;
        public PublishQuizOrchestratorHandler(IMediator mediator, IValidator<PublishQuizOrchestrator> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(PublishQuizOrchestrator request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
                .ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string
                    .Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<bool>
                    .Failure(validationErrors, RequestErrorCode.ValidationError);
            }

            var isQuizPublished = await _mediator
                .Send(new CheckQuizPublishStateQueryRequest(request.quizId), cancellationToken);

            if (isQuizPublished.Data)
            {
                return RequestResult<bool>
                    .Failure("Quiz is already published.", RequestErrorCode.Conflict);
            }

            var QuestionsCount = await _mediator
                .Send(new GetQuestionsCountPerQuizQueryRequest(request.quizId), cancellationToken);

            if (QuestionsCount.Data < 1)
            {
                return RequestResult<bool>
                    .Failure("Quiz has no questions.", RequestErrorCode.Conflict);
            }

            var hasValidQuestions = await _mediator
                .Send(new CheckAllQuizQuestionsHasValidOptionsQueryRequest(request.quizId), cancellationToken);

            if (!hasValidQuestions.Data)
            {
                return RequestResult<bool>
                    .Failure("All questions must have at least 2 options and at least one correct option.", RequestErrorCode.Forbidden);
            }

            var result = await _mediator
                .Send(new UpdateQuizStatusCommandRequest(request.quizId, QuizStatus.Published), cancellationToken);

            if (!result.Data)
            {
                return RequestResult<bool>
                    .Failure("Failed to publish the quiz.", RequestErrorCode.InternalServerError);
            }

            return result;
        }
    }

}
