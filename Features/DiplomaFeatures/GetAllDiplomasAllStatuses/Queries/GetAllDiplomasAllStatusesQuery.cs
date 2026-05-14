using ExaminationSystem.Features.DiplomaFeatures.GetAllDiplomas.Queries.DTOS;

namespace ExaminationSystem.Features.DiplomaFeatures.GetAllDiplomasAllStatuses.Queries
{
    public record GetAllDiplomasAllStatusesQuery(int Page = 1, int PerPage = 20)
        : IRequest<RequestResult<GetAllDiplomaPaginatedDTO>>;

    public class GetAllDiplomasAllStatusesQueryValidator : AbstractValidator<GetAllDiplomasAllStatusesQuery>
    {
        public GetAllDiplomasAllStatusesQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PerPage)
                .GreaterThan(0).WithMessage("PerPage must be greater than 0");
        }
    }

    public class GetAllDiplomasAllStatusesQueryHandler
        : IRequestHandler<GetAllDiplomasAllStatusesQuery, RequestResult<GetAllDiplomaPaginatedDTO>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IValidator<GetAllDiplomasAllStatusesQuery> _validator;

        public GetAllDiplomasAllStatusesQueryHandler(
            IGeneralRepository<Diploma> diplomaRepository,
            IValidator<GetAllDiplomasAllStatusesQuery> validator)
        {
            _diplomaRepository = diplomaRepository;
            _validator = validator;
        }

        public async Task<RequestResult<GetAllDiplomaPaginatedDTO>> Handle(
            GetAllDiplomasAllStatusesQuery request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator
                .ValidateRequestAsync<GetAllDiplomasAllStatusesQuery, GetAllDiplomaPaginatedDTO>(
                    request,
                    cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var diplomas = _diplomaRepository
                .GetAll()
                .Select(d => new GetPublishedDiplomaResponseDTO(
                    d.Id,
                    d.Title,
                    d.Description,
                    d.Status,
                    d.Quizzes.Count(q => !q.isDeleted)));

            if (!await diplomas.AnyAsync(cancellationToken))
            {
                return RequestResult<GetAllDiplomaPaginatedDTO>.Success(
                    new GetAllDiplomaPaginatedDTO([], request.Page, request.PerPage, 0, 0),
                    "No diplomas found",
                    RequestErrorCode.Success);
            }

            var (data, total, totalPages) = await diplomas
                .ToPaginatedAsync(request.Page, request.PerPage, cancellationToken);

            var responseDTOs = new GetAllDiplomaPaginatedDTO(
                data,
                request.Page,
                request.PerPage,
                total,
                totalPages);

            return RequestResult<GetAllDiplomaPaginatedDTO>.Success(
                responseDTOs,
                "Diplomas retrieved successfully",
                RequestErrorCode.Success);
        }
    }
}
