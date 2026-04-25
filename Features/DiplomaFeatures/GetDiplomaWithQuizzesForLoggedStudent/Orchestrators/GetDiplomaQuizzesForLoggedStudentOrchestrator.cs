using ExaminationSystem.Features.Common.DiplomaRequests.Queries;
using ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent.Orchestrators.DTOS;
using ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent.Queries;
using ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent.Queries.GetDiplomaQuizzes;
using ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent.Queries.GetStudentQuizzesAttemptsForDiploma;



namespace ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent.Orchestrators
{
    public record GetDiplomaQuizzesForLoggedStudentOrchestrator(Guid diplomaId, string StudentId)
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
            var validationResult = await _validator
              .ValidateRequestAsync<GetDiplomaQuizzesForLoggedStudentOrchestrator,
              List<GetDiplomaQuizzesForLoggedStudentDTO>>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var isDiplomaExistAndPublished = await _mediator
                .Send(new CheckDiplomaExistAndPublishedQuery(request.diplomaId), cancellationToken);

            if (!isDiplomaExistAndPublished.IsSuccess)
            {
                return RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>
                .Failure(isDiplomaExistAndPublished.Message, isDiplomaExistAndPublished.requestErrorCode);
            }

            //var LoggedStudentIdResult = await _mediator
            //    .Send(new GetCurrentLoggedStudentIdRequest(), cancellationToken);

            //if (!LoggedStudentIdResult.IsSuccess)
            //{
            //    return RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>
            //   .Failure(LoggedStudentIdResult.Message, LoggedStudentIdResult.requestErrorCode);
            //}

            var isEnrolledResult = await _mediator
                .Send(new CheckLoggedStudentEnrolledInDiplomaQueryRequest(request.diplomaId, request.StudentId), cancellationToken);

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

            var diplomaQuizzes = DiplomaQuizzesResult.Data!;


            var quizzesAttemptsResult = await _mediator
              .Send(new GetStudentQuizzesAttemptsForDiplomaQueryRequest(
              request.diplomaId, request.StudentId), cancellationToken);

            if (!quizzesAttemptsResult.IsSuccess)
                return RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>
                    .Failure(quizzesAttemptsResult.Message, quizzesAttemptsResult.requestErrorCode);

            var quizzesAttempts = quizzesAttemptsResult.Data!;


            var attemptsByQuiz = quizzesAttempts
                .GroupBy(a => a.QuizId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var result = diplomaQuizzes
                .Select(quiz =>
                {
                    var attempts = attemptsByQuiz.GetValueOrDefault(quiz.QuizId) ?? new();
                    var attemptCount = attempts.Count;
                    var canAttempt = quiz.MaxAttempts == null || attemptCount < quiz.MaxAttempts;
                    var lastScore = attempts.FirstOrDefault()?.AttemptScore;

                    return new GetDiplomaQuizzesForLoggedStudentDTO(
                        quiz.QuizId,
                        quiz.Title,
                        attemptCount,
                        quiz.DurationInMinutes,
                        quiz.PassScore,
                        quiz.MaxAttempts,
                        canAttempt,
                        lastScore,
                        quiz.Status
                    );
                }).ToList();

            return RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>.Success(result);


        }
    }
}




