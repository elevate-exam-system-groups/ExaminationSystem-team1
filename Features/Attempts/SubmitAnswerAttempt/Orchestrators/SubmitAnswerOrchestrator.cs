using ExaminationSystem.Features.Attempts.Shared.Orchestrators;
using ExaminationSystem.Features.Attempts.Shared.Queries;
using ExaminationSystem.Features.Attempts.SubmitAnswerAttempt.Commands;
using ExaminationSystem.Features.Attempts.SubmitAnswerAttempt.Queries;

namespace ExaminationSystem.Features.Attempts.SubmitAnswerAttempt.Orchestrators
{
    public record SubmitAnswerOrchestrator(string StudentId, Guid attemptId, Guid QuestionId, Guid SelectedOptionId)
        : IRequest<RequestResult<bool>>;

    public class SubmitAnswerOrchestratorValidator : AbstractValidator<SubmitAnswerOrchestrator>
    {
        public SubmitAnswerOrchestratorValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty().WithMessage("QuestionId is required");
            RuleFor(x => x.SelectedOptionId)
                .NotEmpty().WithMessage("SelectedOptionId is required");
            RuleFor(x => x.attemptId)
                .NotEmpty().WithMessage("AttemptId is required");
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("StudentId is required");
        }

    }


    public class SubmitAnswerOrchestratorHandler
        : IRequestHandler<SubmitAnswerOrchestrator, RequestResult<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IValidator<SubmitAnswerOrchestrator> _validator;

        public SubmitAnswerOrchestratorHandler(IMediator mediator, IValidator<SubmitAnswerOrchestrator> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        public async Task<RequestResult<bool>> Handle(SubmitAnswerOrchestrator request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
                   .ValidateRequestAsync<SubmitAnswerOrchestrator, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var isStudentOwnAttempt = await _mediator
           .Send(new CheckStudentAttemptOwnershipQuery(request.attemptId, request.StudentId), cancellationToken);

            if (!isStudentOwnAttempt.IsSuccess)
            {
                return RequestResult<bool>
                    .Failure(isStudentOwnAttempt.Message,
                    isStudentOwnAttempt.requestErrorCode);
            }

            var CheckInProgressStatus = await _mediator
            .Send(new IsStudentAttemptInProgressQuery(request.attemptId, request.StudentId), cancellationToken);

            if (!CheckInProgressStatus.IsSuccess)
            {
                return RequestResult<bool>
                    .Failure(CheckInProgressStatus.Message,
                    CheckInProgressStatus.requestErrorCode);
            }

            var isTimerValid = await _mediator
                .Send(new IsQuizTimerExpiredQuery(request.attemptId, request.StudentId), cancellationToken);

            if (isTimerValid.IsSuccess && isTimerValid.Data == true)
            {
                var AutoSubmitQuizCommandResult = await _mediator
                    .Send(new CompleteQuizAttemptOrchestrator(request.attemptId, QuizAttemptStatus.TimedOut), cancellationToken);

                if (!AutoSubmitQuizCommandResult.IsSuccess)
                {
                    return RequestResult<bool>
                        .Failure(AutoSubmitQuizCommandResult.Message,
                        AutoSubmitQuizCommandResult.requestErrorCode);
                }


                return RequestResult<bool>
                    .Failure("Timer has been elapsed", RequestErrorCode.Gone);
            }

            var quizID = await _mediator
               .Send(new GetQuizIdByQuestionIdQuery(request.QuestionId), cancellationToken);

            if (!quizID.IsSuccess)
                return RequestResult<bool>
                    .Failure(quizID.Message, quizID.requestErrorCode);


            var isQuestionBelongToQuiz = await _mediator
                .Send(new DoesQuestionBelongToQuizQuery(request.QuestionId, quizID.Data), cancellationToken);

            if (!isQuestionBelongToQuiz.IsSuccess)
            {
                return RequestResult<bool>
                    .Failure(isQuestionBelongToQuiz.Message,
                    isQuestionBelongToQuiz.requestErrorCode);
            }

            var isOptionBelongToQuestion = await _mediator
                .Send(new DoesOptionBelongToQuestionQuery(request.SelectedOptionId, request.QuestionId), cancellationToken);

            if (!isOptionBelongToQuestion.IsSuccess)
            {
                return RequestResult<bool>
                    .Failure(isOptionBelongToQuestion.Message,
                    isOptionBelongToQuestion.requestErrorCode);
            }

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
