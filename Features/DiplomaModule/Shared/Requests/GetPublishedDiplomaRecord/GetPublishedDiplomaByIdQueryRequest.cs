using ExaminationSystem.Features.DiplomaModule.Shared.Requests.GetPublishedDiplomaRecord.DTOS;

namespace ExaminationSystem.Features.DiplomaModule.Shared.Requests.GetPublishedDiplomaRecord
{
    public record GetPublishedDiplomaByIdQueryRequest(Guid DiplomaId)
        : IRequest<RequestResult<GetPublishedDiplomaByIdDTO>>;

    public class GetDiplomaByIdQueryRequestValidator : AbstractValidator<GetPublishedDiplomaByIdQueryRequest>
    {
        public GetDiplomaByIdQueryRequestValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }

    public class GetDiplomaByIdQueryHandler
        : IRequestHandler<GetPublishedDiplomaByIdQueryRequest, RequestResult<GetPublishedDiplomaByIdDTO>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IValidator<GetPublishedDiplomaByIdQueryRequest> _validator;
        public GetDiplomaByIdQueryHandler(IGeneralRepository<Diploma> diplomaRepository, IValidator<GetPublishedDiplomaByIdQueryRequest> validator)
        {
            _diplomaRepository = diplomaRepository;
            _validator = validator;
        }
        public async Task<RequestResult<GetPublishedDiplomaByIdDTO>> Handle(GetPublishedDiplomaByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ",
                                            validationResult.Errors.Select(e => e.ErrorMessage));

                return RequestResult<GetPublishedDiplomaByIdDTO>
                       .Failure(validationErrors, RequestErrorCode.ValidationError);
            }

            var diploma = _diplomaRepository
                                         .Get(d => d.Id == request.DiplomaId && d.Status == DiplomaStatus.Published)
                                         .Select(d => new
                                         {
                                             d.Id,
                                             d.Title,
                                             d.Description,
                                             d.Status,
                                         })
                                         .FirstOrDefault();

            if (diploma is null)
            {
                return RequestResult<GetPublishedDiplomaByIdDTO>
                       .Failure("Diploma not found", RequestErrorCode.NotFound);
            }
            var responseDTO = new GetPublishedDiplomaByIdDTO(
                diploma.Id,
                diploma.Title,
                diploma.Description,
                diploma.Status);

            return RequestResult<GetPublishedDiplomaByIdDTO>
                   .Success(responseDTO, "Diploma retrieved successfully", RequestErrorCode.Success);
        }
    }

}
