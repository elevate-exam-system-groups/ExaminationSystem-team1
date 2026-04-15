using ExaminationSystem.Features.DiplomaModule.CreateDiploma.DTOS;
using FluentValidation;

namespace ExaminationSystem.Features.DiplomaModule.CreateDiploma.Requests
{
    #region Request

    public record CreateDiplomaCommandRequest(string Title, string? Description) : IRequest<RequestResult<CreateDiplomaResponseDTO>> { }

    #endregion

    #region Validator

    public class CreateDiplomaCommandValidator : AbstractValidator<CreateDiplomaCommandRequest>
    {
        public CreateDiplomaCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .Length(3, 200).WithMessage("Description must be between 3 and 200 characters");
        }
    }
    #endregion

    #region Handler

    public class CreateDiplomaCommandHandler : IRequestHandler<CreateDiplomaCommandRequest, RequestResult<CreateDiplomaResponseDTO>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IValidator<CreateDiplomaCommandRequest> _validator;
        public CreateDiplomaCommandHandler(IGeneralRepository<Diploma> diplomaRepository, IValidator<CreateDiplomaCommandRequest> validator)
        {
            _diplomaRepository = diplomaRepository;
            _validator = validator;
        }
        public async Task<RequestResult<CreateDiplomaResponseDTO>> Handle(CreateDiplomaCommandRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var result = RequestResult<CreateDiplomaResponseDTO>
                     .Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)),
                     RequestErrorCode.ValidationError);
                return result;

            }
            var newDiploma = new Diploma
            {
                Title = request.Title,
                Description = request.Description
            };

            _diplomaRepository.Add(newDiploma);
            await _diplomaRepository.SaveChangesAsync();

            var responseDTO = new CreateDiplomaResponseDTO(
                newDiploma.Id,
                newDiploma.Title,
                newDiploma.Description,
                newDiploma.Status);

            return RequestResult<CreateDiplomaResponseDTO>.Success(responseDTO, "Diploma created successfully", RequestErrorCode.Success);

        }
    }

    #endregion
}

