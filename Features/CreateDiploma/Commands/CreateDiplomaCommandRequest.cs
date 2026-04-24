using ExaminationSystem.Features.CreateDiploma.Commands.DTOS;

namespace ExaminationSystem.Features.CreateDiploma.Commands
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

    public class CreateDiplomaCommandHandler : BaseRequestHandler<CreateDiplomaCommandRequest, CreateDiplomaResponseDTO>//IRequestHandler<CreateDiplomaCommandRequest, RequestResult<CreateDiplomaResponseDTO>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;

        public CreateDiplomaCommandHandler(IGeneralRepository<Diploma> diplomaRepository,
            HandlerBasicParameterss<CreateDiplomaCommandRequest> paramters) : base(paramters)
        {
            _diplomaRepository = diplomaRepository;
        }
        public override async Task<RequestResult<CreateDiplomaResponseDTO>> Handle(CreateDiplomaCommandRequest request, CancellationToken cancellationToken)
        {

            var ValidationResult = ValidateRequest(request);
            if (!ValidationResult.IsSuccess)
                return ValidationResult;

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

