using ExaminationSystem.Features.Common.DiplomaRequests.Orchestrators.GetPuplishedDiplomaByIdWithQuizzesOrchestrator.DTOS;
using ExaminationSystem.Features.Common.DiplomaRequests.Queries;
using ExaminationSystem.Features.Common.DiplomaRequests.Queries.GetPublishedDiplomaRecord;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Common.DiplomaRequests.Orchestrators.GetPuplishedDiplomaByIdWithQuizzesOrchestrator
{
    public record GetPuplishedDiplomaQuizzesByIDOrchestratorRequest(Guid diplomaId)
        : IRequest<RequestResult<GetPuplishedDiplomaQuizzesByIdDTO>>;


    public class GetPuplishedDiplomaQuizzesByIDOrchestratorRequestValidator
        : AbstractValidator<GetPuplishedDiplomaQuizzesByIDOrchestratorRequest>
    {
        public GetPuplishedDiplomaQuizzesByIDOrchestratorRequestValidator()
        {
            RuleFor(x => x.diplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }

    public class GetPuplishedDiplomaQuizzesByIDOrchestratorHandler
        : IRequestHandler<GetPuplishedDiplomaQuizzesByIDOrchestratorRequest, RequestResult<GetPuplishedDiplomaQuizzesByIdDTO>>
    {
        private readonly IMediator _mediator;
        private readonly IValidator<GetPuplishedDiplomaQuizzesByIDOrchestratorRequest> _validator;


        public GetPuplishedDiplomaQuizzesByIDOrchestratorHandler(IMediator mediator, IValidator<GetPuplishedDiplomaQuizzesByIDOrchestratorRequest> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        public async Task<RequestResult<GetPuplishedDiplomaQuizzesByIdDTO>> Handle(
        GetPuplishedDiplomaQuizzesByIDOrchestratorRequest request,
        CancellationToken cancellationToken)
        {

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ",
                                            validationResult.Errors.Select(e => e.ErrorMessage));

                return RequestResult<GetPuplishedDiplomaQuizzesByIdDTO>
                       .Failure(validationErrors, RequestErrorCode.ValidationError);
            }

            var diplomaRecordResult = await _mediator
                .Send(new GetPublishedDiplomaByIdQueryRequest(request.diplomaId), cancellationToken);

            if (!diplomaRecordResult.IsSuccess)
            {
                return RequestResult<GetPuplishedDiplomaQuizzesByIdDTO>
                    .Failure(diplomaRecordResult.Message ?? "Failed to get diploma record", diplomaRecordResult.requestErrorCode);
            }

            var diplomaRecord = diplomaRecordResult.Data;

            var QuizzesCountResult = await _mediator
                .Send(new GetDiplomaQuizCountQueryRequest(request.diplomaId), cancellationToken);

            if (!QuizzesCountResult.IsSuccess)
            {
                return RequestResult<GetPuplishedDiplomaQuizzesByIdDTO>
                    .Failure(QuizzesCountResult.Message ?? "Failed to get quizzes count", QuizzesCountResult.requestErrorCode);
            }

            var quizzesCount = QuizzesCountResult.Data;

            return RequestResult<GetPuplishedDiplomaQuizzesByIdDTO>
                .Success(new GetPuplishedDiplomaQuizzesByIdDTO
                (
                    diplomaRecord.Id,
                    diplomaRecord.Title,
                    diplomaRecord.Description,
                    diplomaRecord.Status,
                    quizzesCount
                ));
        }
    }


}
