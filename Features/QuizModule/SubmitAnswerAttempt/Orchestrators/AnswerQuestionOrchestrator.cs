using ExaminationSystem.Features.Common.GetCurrentLoggedStudentID;
using ExaminationSystem.Features.QuizModule.Shared;
using ExaminationSystem.Features.QuizModule.SubmitAnswerAttempt.Requests;

namespace ExaminationSystem.Features.QuizModule.SubmitAnswerAttempt.Orchestrators
{
    public record AnswerQuestionOrchestrator(Guid attemptId, Guid QuestionId, Guid SelectedOptionId)
        : IRequest<RequestResult<bool>>;

    public class AnswerQuestionOrchestratorValidator : AbstractValidator<AnswerQuestionOrchestrator>
    {
        public AnswerQuestionOrchestratorValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty().WithMessage("QuestionId is required");
            RuleFor(x => x.SelectedOptionId)
                .NotEmpty().WithMessage("SelectedOptionId is required");
        }

    }


    public class AnswerQuestionOrchestratorHandler
        : IRequestHandler<AnswerQuestionOrchestrator, RequestResult<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IValidator<AnswerQuestionOrchestrator> _validator;

        public AnswerQuestionOrchestratorHandler(IMediator mediator, IValidator<AnswerQuestionOrchestrator> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        public async Task<RequestResult<bool>> Handle(AnswerQuestionOrchestrator request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string
                    .Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<bool>
                    .Failure(validationErrors, RequestErrorCode.ValidationError);
            }

            var LoggedStudentId = await _mediator
                .Send(new GetCurrentLoggedStudentIdRequest(), cancellationToken);

            if (!LoggedStudentId.IsSuccess)
            {
                return RequestResult<bool>
                    .Failure(LoggedStudentId.Message, LoggedStudentId.requestErrorCode);
            }

            var checkAttemptValidationResult = await _mediator
           .Send(new CheckStudentOwnTheAttemptQueryRequest(request.attemptId, LoggedStudentId.Data!), cancellationToken);

            if (!checkAttemptValidationResult.IsSuccess)
            {
                return RequestResult<bool>
                    .Failure(checkAttemptValidationResult.Message,
                    checkAttemptValidationResult.requestErrorCode);
            }


            var isTimerValid = await _mediator
                .Send(new CheckQuizTimerHasElapsedQueryRequest(request.attemptId, LoggedStudentId.Data!), cancellationToken);

            if (!isTimerValid.IsSuccess)
            {
                var AutoSubmitQuizCommandResult = await _mediator
                    .Send(new AutoSubmitAttemptCommandRequest(request.attemptId), cancellationToken);

                return RequestResult<bool>
                    .Failure(isTimerValid.Message, isTimerValid.requestErrorCode);
            }

            //var CheckInProgressStatus = await _mediator
            //    .Send(new CheckInProgressQuizAttemptStatusQuerRequest(request.attemptId, LoggedStudentId.Data!), cancellationToken);

            var quizID = await _mediator
               .Send(new GetQuizIdForCurrentQuestionQueryRequest(request.QuestionId), cancellationToken);

            var isQuestionBelongToQuiz = await _mediator
                .Send(new CheckQuestionBelongsToQuizQueryRequest(request.QuestionId, quizID.Data), cancellationToken);

            var isOptionBelongToQuestion = await _mediator
                .Send(new CheckOptionBelongsToQuestionQueryRequest(request.SelectedOptionId, request.QuestionId), cancellationToken);



            var recordAnswerCommandResult = await _mediator
                .Send(new RecordAnswerCommandRequest(
                    request.QuestionId,
                    request.SelectedOptionId,
                    request.attemptId),
                    cancellationToken);

            if (!recordAnswerCommandResult.IsSuccess)
            {
                return RequestResult<bool>
                    .Failure(recordAnswerCommandResult.Message,
                    recordAnswerCommandResult.requestErrorCode);
            }

            return RequestResult<bool>.Success(recordAnswerCommandResult.Data);
        }
    }
}
