
namespace ExaminationSystem.Features.Common.DiplomaRequests.Queries
{
    public record CheckDiplomaExistAndPublishedQuery(Guid DiplomaId) :
        IRequest<RequestResult<bool>>;

    public class CheckDiplomaExistAndPublishedQueryValidator : AbstractValidator<CheckDiplomaExistAndPublishedQuery>
    {
        public CheckDiplomaExistAndPublishedQueryValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }

    public class CheckDiplomaExistAndPublishedQueryHandler : IRequestHandler<CheckDiplomaExistAndPublishedQuery, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IValidator<CheckDiplomaExistAndPublishedQuery> _validator;
        public CheckDiplomaExistAndPublishedQueryHandler(IGeneralRepository<Diploma> diplomaRepository, IValidator<CheckDiplomaExistAndPublishedQuery> validator)
        {
            _diplomaRepository = diplomaRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(CheckDiplomaExistAndPublishedQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ",
                    validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<bool>.Failure(validationErrors, RequestErrorCode.ValidationError);
            }
            var isExistAndPublished = await _diplomaRepository
                .Get(d => d.Id == request.DiplomaId && d.Status == DiplomaStatus.Published)
                .AnyAsync(cancellationToken);

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
