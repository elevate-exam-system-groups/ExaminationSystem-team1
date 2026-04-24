using ExaminationSystem.Features.Common.AttemptRequests.Queries;
using ExaminationSystem.Features.Common.QuizRequests.Queries;
using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.StartQuizAttempt.Commands;
using ExaminationSystem.Features.StartQuizAttempt.Orchestrators.DTOS;
using ExaminationSystem.Features.StartQuizAttempt.Queries;
using ExaminationSystem.Features.StartQuizAttempt.Queries.GetQuizAttemptMetaData;

namespace ExaminationSystem.Features.StartQuizAttempt.Orchestrators
{
    public record StartQuizAttemptOrchestrator(Guid QuizId, string StudentId)
        : IRequest<RequestResult<StartAttemptDTO>>;
    public class StartQuizAttemptOrchestratorValidator : AbstractValidator<StartQuizAttemptOrchestrator>
    {
        public StartQuizAttemptOrchestratorValidator()
        {
            RuleFor(x => x.QuizId)
                .NotEmpty().WithMessage("Quiz Id is required");

            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Stdent Id is required");
        }
    }
    public class StartQuizAttemptOrchestratorHandler
       : IRequestHandler<StartQuizAttemptOrchestrator, RequestResult<StartAttemptDTO>>
    {
        private readonly IMediator _mediator;
        private readonly IValidator<StartQuizAttemptOrchestrator> _validator;

        public StartQuizAttemptOrchestratorHandler(IMediator mediator, IValidator<StartQuizAttemptOrchestrator> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        public async Task<RequestResult<StartAttemptDTO>> Handle(StartQuizAttemptOrchestrator request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
                .ValidateRequestAsync<StartQuizAttemptOrchestrator, StartAttemptDTO>
                (request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var QuizPublished = await _mediator
                .Send(new IsQuizPublishedQuery(request.QuizId), cancellationToken);

            if (!QuizPublished.Data)
            {
                return RequestResult<StartAttemptDTO>
                    .Failure("Quiz is Unpuplished", RequestErrorCode.NotFound);
            }

            var ValidAttempt = await _mediator
                .Send(new CanStudentAttemptQuizQuery(request.QuizId, request.StudentId), cancellationToken);

            if (!ValidAttempt.Data)
            {
                return RequestResult<StartAttemptDTO>
                    .Failure("Student Cannot Attempt this Quiz", RequestErrorCode.Forbidden);
            }

            var InProgressAttempt = await _mediator
                .Send(new GetStudentInProgressQuizAttemptQuery(request.StudentId, request.QuizId), cancellationToken);

            if (InProgressAttempt.Data.HasValue)
            {
                return RequestResult<StartAttemptDTO>
                       .Failure($"Already in progress, attempt_id: " +
                       $"{InProgressAttempt.Data.Value}", RequestErrorCode.Conflict);
            }


            var CreateAttemptResult = await _mediator
            .Send(new CreateAttemptRecordCommand(request.QuizId, request.StudentId), cancellationToken);

            if (!CreateAttemptResult.IsSuccess)
            {
                return RequestResult<StartAttemptDTO>
                    .Failure(CreateAttemptResult.Message, CreateAttemptResult.requestErrorCode);
            }

            var metaData = await _mediator
               .Send(new GetQuizMetaDataQuery(request.QuizId), cancellationToken);

            if (!metaData.IsSuccess)
            {
                return RequestResult<StartAttemptDTO>
                    .Failure(metaData.Message, metaData.requestErrorCode);
            }

            var shuffledQuestions = metaData.Data.Questions
                        .OrderBy(_ => Guid.NewGuid())
                        .Select(q => q with
                        {
                            Options = q.Options
                           .OrderBy(_ => Guid.NewGuid())
                           .ToList()
                        })
                           .ToList();

            var shuffledMeta = metaData.Data with
            {
                Questions = shuffledQuestions
            };

            var startAttemptDTO = new StartAttemptDTO
            (
                CreateAttemptResult.Data,
                shuffledMeta
            );

            return RequestResult<StartAttemptDTO>.Success(startAttemptDTO);
        }
    }
}


