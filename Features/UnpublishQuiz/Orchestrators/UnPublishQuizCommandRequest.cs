using ExaminationSystem.Features.Common.QuizRequests.Commands;
using ExaminationSystem.Features.Common.QuizRequests.Queries;
using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.UnpublishQuiz.Queries;


namespace ExaminationSystem.Features.UnpublishQuiz.Orchestrators
{
    public record UnPublishQuizCommandRequest(Guid quizId) : IRequest<RequestResult<bool>>;


    public class UnPublishQuizCommandRequestValidator : AbstractValidator<UnPublishQuizCommandRequest>
    {
        public UnPublishQuizCommandRequestValidator()
        {
            RuleFor(r => r.quizId)
                .NotEmpty()
                .WithMessage("Quiz Id is required.");
        }
    }

    public class UnPublishQuizCommandRequestHandler : IRequestHandler<UnPublishQuizCommandRequest, RequestResult<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IValidator<UnPublishQuizCommandRequest> _validator;
        public UnPublishQuizCommandRequestHandler(IMediator mediator, IValidator<UnPublishQuizCommandRequest> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(UnPublishQuizCommandRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
              .ValidateRequestAsync<UnPublishQuizCommandRequest, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var isQuizPublished = await _mediator
                .Send(new IsQuizPublishedQuery(request.quizId), cancellationToken);

            if (!isQuizPublished.Data)
            {
                return RequestResult<bool>
                    .Failure("Quiz is already unpublished.", RequestErrorCode.Conflict);
            }

            var hasAttempts = await _mediator
                .Send(new CheckQuizHasCurrentAttemptsQueryRequest(request.quizId), cancellationToken);

            if (hasAttempts.Data)
            {
                return RequestResult<bool>
                    .Failure("Cannot unpublish quiz with attempts.", RequestErrorCode.Conflict);
            }

            var result = await _mediator
                .Send(new UpdateQuizStatusCommandRequest(request.quizId, QuizStatus.Draft), cancellationToken);

            if (!result.Data)
            {
                return RequestResult<bool>
                    .Failure("Failed to unpublish quiz.", RequestErrorCode.InternalServerError);
            }

            return RequestResult<bool>.Success(true);
        }
    }

}
