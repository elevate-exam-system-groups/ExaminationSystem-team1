
namespace ExaminationSystem.Features.DeleteDiploma.Commands
{
    public record DeleteDiplomaCommand(Guid DiplomaId) : IRequest<RequestResult<bool>> { }

    #region Validator

    public class DeleteDiplomaCommandValidator : AbstractValidator<DeleteDiplomaCommand>
    {
        public DeleteDiplomaCommandValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }
    #endregion

    #region Handler

    public class DeleteDiplomaCommandHandler : IRequestHandler<DeleteDiplomaCommand, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IValidator<DeleteDiplomaCommand> _validator;

        public DeleteDiplomaCommandHandler(IGeneralRepository<Diploma> diplomaRepository, IValidator<DeleteDiplomaCommand> validator)
        {
            _diplomaRepository = diplomaRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(DeleteDiplomaCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
              .ValidateRequestAsync<DeleteDiplomaCommand, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var existingDiploma = _diplomaRepository.GetById(request.DiplomaId).FirstOrDefault();

            if (existingDiploma is null)
            {
                return RequestResult<bool>.Failure("Diploma not found", RequestErrorCode.NotFound);
            }

            var hasEnrollments = _diplomaRepository.Get(d => d.Id == request.DiplomaId)
                                               .SelectMany(d => d.Enrollments)
                                               .Any();
            if (hasEnrollments)
            {
                return RequestResult<bool>.Failure("Cannot delete diploma with existing enrollments",
                                                    RequestErrorCode.Conflict);
            }

            _diplomaRepository.SoftDelete(existingDiploma);
            await _diplomaRepository.SaveChangesAsync();
            return RequestResult<bool>.Success(true);
        }
    }

    #endregion

}
