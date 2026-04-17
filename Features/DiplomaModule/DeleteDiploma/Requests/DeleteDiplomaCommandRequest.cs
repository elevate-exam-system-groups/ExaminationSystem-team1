
namespace ExaminationSystem.Features.DiplomaModule.DeleteDiploma.Requests
{
    public record DeleteDiplomaCommandRequest(Guid DiplomaId) : IRequest<RequestResult<bool>> { }

    #region Validator

    public class DeleteDiplomaCommandValidator : AbstractValidator<DeleteDiplomaCommandRequest>
    {
        public DeleteDiplomaCommandValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }
    #endregion

    #region Handler

    public class DeleteDiplomaCommandHandler : IRequestHandler<DeleteDiplomaCommandRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IGeneralRepository<Enrollment> _enrollmentRepository;
        private readonly IValidator<DeleteDiplomaCommandRequest> _Validator;

        public DeleteDiplomaCommandHandler(IGeneralRepository<Diploma> diplomaRepository, IGeneralRepository<Enrollment> enrollmentRepository, IValidator<DeleteDiplomaCommandRequest> validator)
        {
            _diplomaRepository = diplomaRepository;
            _enrollmentRepository = enrollmentRepository;
            _Validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(DeleteDiplomaCommandRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _Validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                var result = RequestResult<bool>
                                            .Failure(validationErrors, RequestErrorCode.ValidationError);
                return result;
            }

            var existingDiploma = _diplomaRepository.GetById(request.DiplomaId).FirstOrDefault();

            if (existingDiploma is null)
            {
                return RequestResult<bool>.Failure("Diploma not found", RequestErrorCode.NotFound);
            }

            #region Check Using diploma repo
            //var hasEnrollments = _diplomaRepository.GetById(request.DiplomaId)
            //                                   .SelectMany(d => d.Enrollments)
            //                                   .Any();
            // if (hasEnrollments)
            //{
            //    return RequestResult<bool>.Failure("Cannot delete diploma with existing enrollments", 
            //                                        RequestErrorCode.Conflict);
            //} 
            #endregion

            var hasEnrollments = _enrollmentRepository.Get(e => e.DiplomaId == request.DiplomaId).Any();

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
