using ExaminationSystem.Features.DiplomaModule.GetPublishedDiplomaRecord.DTOS;

namespace ExaminationSystem.Features.DiplomaModule.GetPublishedDiplomaRecord
{
    public record GetPublishedDiplomaByIdQueryRequest(Guid DiplomaId) : IRequest<RequestResult<GetPublishedDiplomaByIDResponseDTO>>;

    public class GetDiplomaByIdQueryRequestValidator : AbstractValidator<GetPublishedDiplomaByIdQueryRequest>
    {
        public GetDiplomaByIdQueryRequestValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }

    public class GetDiplomaByIdQueryHandler : IRequestHandler<GetPublishedDiplomaByIdQueryRequest, RequestResult<GetPublishedDiplomaByIDResponseDTO>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IValidator<GetPublishedDiplomaByIdQueryRequest> _validator;
        public GetDiplomaByIdQueryHandler(IGeneralRepository<Diploma> diplomaRepository, IValidator<GetPublishedDiplomaByIdQueryRequest> validator)
        {
            _diplomaRepository = diplomaRepository;
            _validator = validator;
        }
        public async Task<RequestResult<GetPublishedDiplomaByIDResponseDTO>> Handle(GetPublishedDiplomaByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ",
                                            validationResult.Errors.Select(e => e.ErrorMessage));

                return RequestResult<GetPublishedDiplomaByIDResponseDTO>
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
                                             QuizzesCount = d.Quizzes.Count(q => !q.isDeleted)
                                         })
                                         .FirstOrDefault();

            if (diploma is null)
            {
                return RequestResult<GetPublishedDiplomaByIDResponseDTO>
                       .Failure("Diploma not found", RequestErrorCode.NotFound);
            }
            var responseDTO = new GetPublishedDiplomaByIDResponseDTO(
                diploma.Id,
                diploma.Title,
                diploma.Description,
                diploma.Status,
                diploma.QuizzesCount);

            return RequestResult<GetPublishedDiplomaByIDResponseDTO>
                   .Success(responseDTO, "Diploma retrieved successfully", RequestErrorCode.Success);
        }
    }

}
