using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Common.DiplomaRequests.Queries
{
    public record CheckDiplomaExistAndPublishedQueryRequest(Guid DiplomaId) :
        IRequest<RequestResult<bool>>;

    public class CheckDiplomaExistAndPublishedQueryRequestValidator : AbstractValidator<CheckDiplomaExistAndPublishedQueryRequest>
    {
        public CheckDiplomaExistAndPublishedQueryRequestValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }

    public class CheckDiplomaExistAndPublishedQueryHandler : IRequestHandler<CheckDiplomaExistAndPublishedQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IValidator<CheckDiplomaExistAndPublishedQueryRequest> _validator;
        public CheckDiplomaExistAndPublishedQueryHandler(IGeneralRepository<Diploma> diplomaRepository, IValidator<CheckDiplomaExistAndPublishedQueryRequest> validator)
        {
            _diplomaRepository = diplomaRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(CheckDiplomaExistAndPublishedQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ",
                    validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<bool>.Failure(validationErrors, RequestErrorCode.ValidationError);
            }
            var isExistAndPublished = _diplomaRepository
                .Get(d => d.Id == request.DiplomaId && d.Status == DiplomaStatus.Published)
                .Any();

            if (!isExistAndPublished)
            {
                return RequestResult<bool>
                    .Failure("Diploma does not exist or is not published", RequestErrorCode.NotFound);
            }

            return RequestResult<bool>
                .Success(isExistAndPublished, "Diploma existence and publication check completed successfully", RequestErrorCode.Success);
        }
    }
}
