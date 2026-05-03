using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.DiplomaFeatures.SharedDiploma.Queries
{
    public record CheckDiplomaHasEnrollmentsQueryRequest(Guid DiplomaId) : IRequest<RequestResult<bool>>;

    public class CheckDiplomaHasEnrollmentsQueryRequestValidator : AbstractValidator<CheckDiplomaHasEnrollmentsQueryRequest>
    {
        public CheckDiplomaHasEnrollmentsQueryRequestValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }

    public class CheckDiplomaHasEnrollmentsQueryHandler : IRequestHandler<CheckDiplomaHasEnrollmentsQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IValidator<CheckDiplomaHasEnrollmentsQueryRequest> _validator;
        public CheckDiplomaHasEnrollmentsQueryHandler(IGeneralRepository<Diploma> diplomaRepository, IValidator<CheckDiplomaHasEnrollmentsQueryRequest> validator)
        {
            _diplomaRepository = diplomaRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(CheckDiplomaHasEnrollmentsQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ",
                    validationResult.Errors.Select(e => e.ErrorMessage));

                return RequestResult<bool>.Failure(validationErrors, RequestErrorCode.ValidationError);
            }

            var hasEnrollments = _diplomaRepository
                .Get(d => d.Id == request.DiplomaId && d.Enrollments.Any())
                .Any();

            return RequestResult<bool>.Success(hasEnrollments, "Enrollment check completed successfully", RequestErrorCode.Success);
        }
    }
}
