using ExaminationSystem.Features.Common.GetDiplomaRecord.DTOS;
using FluentValidation;

namespace ExaminationSystem.Features.Common.GetDiplomaRecord
{
    public record GetDiplomaByIdQueryRequest(Guid DiplomaId) : IRequest<RequestResult<GetDiplomaResponseDTO>>;

    public class GetDiplomaByIdQueryRequestValidator : AbstractValidator<GetDiplomaByIdQueryRequest>
    {
        public GetDiplomaByIdQueryRequestValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }

    public class GetDiplomaByIdQueryHandler : IRequestHandler<GetDiplomaByIdQueryRequest, RequestResult<GetDiplomaResponseDTO>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IValidator<GetDiplomaByIdQueryRequest> _validator;
        public GetDiplomaByIdQueryHandler(IGeneralRepository<Diploma> diplomaRepository, IValidator<GetDiplomaByIdQueryRequest> validator)
        {
            _diplomaRepository = diplomaRepository;
            _validator = validator;
        }
        public async Task<RequestResult<GetDiplomaResponseDTO>> Handle(GetDiplomaByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<GetDiplomaResponseDTO>.Failure(validationErrors, RequestErrorCode.ValidationError);
            }
            var diploma = _diplomaRepository.GetById(request.DiplomaId).FirstOrDefault();
            if (diploma is null)
            {
                return RequestResult<GetDiplomaResponseDTO>.Failure("Diploma not found", RequestErrorCode.NotFound);
            }
            var responseDTO = new GetDiplomaResponseDTO(diploma.Id, diploma.Title, diploma.Description, diploma.Status);
            return RequestResult<GetDiplomaResponseDTO>.Success(responseDTO, "Diploma retrieved successfully", RequestErrorCode.Success);
        }
    }

}
