using ExaminationSystem.Features.Common.GetCurrentLoggedStudentID;
using ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzesForSignedInStudent.DTOS;
using ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzesForSignedInStudent.Requests;
using ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzesForSignedInStudent.Requests.GetQuizSummaryForStudent;
using ExaminationSystem.Features.DiplomaModule.Shared.Requests;
using ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzes.Requests.GetDiplomaQuizzesForLoggedStudent;


namespace ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzesForSignedInStudent.Orchestrators
{
    public record GetDiplomaQuizzesForLoggedStudentOrchestrator(Guid diplomaId)
        : IRequest<RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>>;

    public class GetDiplomaQuizzesForLoggedStudentOrchestratorValidator : AbstractValidator<GetDiplomaQuizzesForLoggedStudentOrchestrator>
    {
        public GetDiplomaQuizzesForLoggedStudentOrchestratorValidator()
        {
            RuleFor(x => x.diplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }

    public class GetDiplomaQuizzesForLoggedStudentOrchestratorHandler
        : IRequestHandler<GetDiplomaQuizzesForLoggedStudentOrchestrator,
            RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>>
    {
        private readonly IMediator _mediator;
        private readonly IValidator<GetDiplomaQuizzesForLoggedStudentOrchestrator> _validator;

        public GetDiplomaQuizzesForLoggedStudentOrchestratorHandler(IMediator mediator
            , IValidator<GetDiplomaQuizzesForLoggedStudentOrchestrator> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }
        public async Task<RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>> Handle(
            GetDiplomaQuizzesForLoggedStudentOrchestrator request, CancellationToken cancellationToken)
        {
            var isDiplomaExistAndPublished = await _mediator
                .Send(new CheckDiplomaExistAndPublishedQueryRequest(request.diplomaId), cancellationToken);

            if (!isDiplomaExistAndPublished.IsSuccess)
            {
                return RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>
                .Failure(isDiplomaExistAndPublished.Message, isDiplomaExistAndPublished.requestErrorCode);
            }

            var LoggedStudentIdResult = await _mediator
                .Send(new GetCurrentLoggedStudentIdRequest(), cancellationToken);

            if (!LoggedStudentIdResult.IsSuccess)
            {
                return RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>
               .Failure(LoggedStudentIdResult.Message, LoggedStudentIdResult.requestErrorCode);
            }

            var isEnrolledResult = await _mediator
                .Send(new CheckLoggedStudentEnrolledInDiplomaQueryRequest(request.diplomaId, LoggedStudentIdResult.Data!), cancellationToken);

            if (!isEnrolledResult.IsSuccess)
            {
                return RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>
               .Failure(isEnrolledResult.Message, isEnrolledResult.requestErrorCode);
            }

            var DiplomaQuizzesResult = await _mediator
               .Send(new GetDiplomaQuizzesQueryRequest(request.diplomaId), cancellationToken);

            if (!DiplomaQuizzesResult.IsSuccess)
            {
                return RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>
               .Failure(DiplomaQuizzesResult.Message, DiplomaQuizzesResult.requestErrorCode);
            }

            var DiplomaQuizzes = DiplomaQuizzesResult.Data!;


            //var QuizzesAttemptsResult = await _mediator
            //    .Send(new GetStudentQuizzesAttemptsForDiplomaQueryRequest(request.diplomaId,
            //         LoggedStudentIdResult.Data!), cancellationToken);

            //if (!QuizzesAttemptsResult.IsSuccess)
            //{
            //    return RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>
            //   .Failure(QuizzesAttemptsResult.Message, QuizzesAttemptsResult.requestErrorCode);
            //}

            //var QuizzesAttempts = QuizzesAttemptsResult.Data!;

            var QuizSummaryResultresponse = await Task.WhenAll(
             DiplomaQuizzes.Select(quiz => _mediator
                           .Send(new GetQuizSummaryForStudentQueryRequest(
             quiz.QuizId,
            LoggedStudentIdResult.Data!,
            quiz.MaxAttempts
             ), cancellationToken))
);

            var result = DiplomaQuizzes.Select((quiz, index) =>
     new GetDiplomaQuizzesForLoggedStudentDTO(
         quiz.QuizId,
         quiz.Title,
         QuizSummaryResultresponse[index].Data!.AttemptCount,
         quiz.DurationInMinutes,
         quiz.PassScore,
         quiz.MaxAttempts,
         QuizSummaryResultresponse[index].Data!.CanAttempt,
         QuizSummaryResultresponse[index].Data!.LastScore,
         quiz.Status
     )).ToList();

            return RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>.Success(result);


        }
    }
}




