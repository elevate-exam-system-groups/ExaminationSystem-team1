using ExaminationSystem.Features.Common.GetDiplomaRecord.DTOS;

namespace ExaminationSystem.Features.Common.GetDiplomaRecord
{
    public record GetDiplomaByIdQueryRequest(Guid DiplomaId) : IRequest<RequestResult<GetDiplomaByIDResponseDTO>>;

    public class GetDiplomaByIdQueryRequestValidator : AbstractValidator<GetDiplomaByIdQueryRequest>
    {
        public GetDiplomaByIdQueryRequestValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }

    public class GetDiplomaByIdQueryHandler : IRequestHandler<GetDiplomaByIdQueryRequest, RequestResult<GetDiplomaByIDResponseDTO>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IValidator<GetDiplomaByIdQueryRequest> _validator;
        public GetDiplomaByIdQueryHandler(IGeneralRepository<Diploma> diplomaRepository, IValidator<GetDiplomaByIdQueryRequest> validator)
        {
            _diplomaRepository = diplomaRepository;
            _validator = validator;
        }
        public async Task<RequestResult<GetDiplomaByIDResponseDTO>> Handle(GetDiplomaByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<GetDiplomaByIDResponseDTO>.Failure(validationErrors, RequestErrorCode.ValidationError);
            }
            var diploma = _diplomaRepository.GetById(request.DiplomaId).FirstOrDefault();
            if (diploma is null)
            {
                return RequestResult<GetDiplomaByIDResponseDTO>.Failure("Diploma not found", RequestErrorCode.NotFound);
            }
            var responseDTO = new GetDiplomaByIDResponseDTO(
                diploma.Id,
                diploma.Title,
                diploma.Description,
                diploma.Status,
                diploma.Quizzes.Count(q => !q.isDeleted));

            return RequestResult<GetDiplomaByIDResponseDTO>.Success(responseDTO, "Diploma retrieved successfully", RequestErrorCode.Success);
        }
    }

}
