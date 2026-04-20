using ExaminationSystem.Features.DiplomaModule.UpdateDiploma.DTOS;

namespace ExaminationSystem.Features.DiplomaModule.UpdateDiploma.Requests
{
    public record UpdateDiplomaCommandRequest(Guid id, string? Title, string? Description) : IRequest<RequestResult<UpdateDiplomaResponseDTO>> { }

    public class UpdateDiplomaCommandValidator : AbstractValidator<UpdateDiplomaCommandRequest>
    {
        public UpdateDiplomaCommandValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.Title)
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters")
                .When(x => x.Title is not null);
            RuleFor(x => x.Description)
                .Length(3, 200).WithMessage("Description must be between 3 and 200 characters")
                .When(x => x.Description is not null);
        }
    }

    public class UpdateDiplomaCommandHandler : IRequestHandler<UpdateDiplomaCommandRequest, RequestResult<UpdateDiplomaResponseDTO>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IValidator<UpdateDiplomaCommandRequest> _validator;

        public UpdateDiplomaCommandHandler(IGeneralRepository<Diploma> diplomaRepository, IValidator<UpdateDiplomaCommandRequest> validator)
        {
            _diplomaRepository = diplomaRepository;
            _validator = validator;
        }

        public async Task<RequestResult<UpdateDiplomaResponseDTO>> Handle(UpdateDiplomaCommandRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<UpdateDiplomaResponseDTO>
                    .Failure(validationErrors, RequestErrorCode.ValidationError);
            }

            var existingDiploma = new Diploma
            {
                Id = request.id,
                Title = request?.Title,
                Description = request?.Description
            };

            if (existingDiploma is null)
            {
                return RequestResult<UpdateDiplomaResponseDTO>
                    .Failure("Diploma not found", RequestErrorCode.NotFound);
            }

            _diplomaRepository.UpdateInclude(existingDiploma, nameof(existingDiploma.Title), nameof(existingDiploma.Description));
            await _diplomaRepository.SaveChangesAsync();

            var response = new UpdateDiplomaResponseDTO(existingDiploma.Id, existingDiploma.Title, existingDiploma.Description, existingDiploma.Status);
            return RequestResult<UpdateDiplomaResponseDTO>.Success(response, "Diploma updated successfully", RequestErrorCode.UpdateSuccess);
        }
    }
}