using ExaminationSystem.Features.Common.Helpers;
using ExaminationSystem.Features.QuizFeatures.PublishQuiz.Queries;
using ExaminationSystem.Features.QuizFeatures.SharedQuizzes.Commands;
using ExaminationSystem.Features.QuizFeatures.SharedQuizzes.Queries;

namespace ExaminationSystem.Features.QuizFeatures.PublishQuiz.Orchestrators
{
    public record PublishQuizOrchestrator(Guid quizId) : ICommand<RequestResult<bool>>;
    //: IRequest<RequestResult<bool>>;

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

            var isQuizPublished = await _mediator
                .Send(new IsQuizPublishedQuery(request.quizId), cancellationToken);

            if (isQuizPublished.Data)
            {
                return RequestResult<bool>
                    .Failure("Quiz is already published.", RequestErrorCode.Conflict);
            }

            var QuestionsCount = await _mediator
                .Send(new GetQuestionsCountPerQuizQuery(request.quizId), cancellationToken);

            if (QuestionsCount.Data < 1)
            {
                return RequestResult<bool>
                    .Failure("Quiz has no questions.", RequestErrorCode.Conflict);
            }

            var hasValidQuestions = await _mediator
                .Send(new CheckAllQuizQuestionsHasValidOptionsQuery(request.quizId), cancellationToken);

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
