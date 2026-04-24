using ExaminationSystem.Features.GetAllDiplomas.Queries.DTOS;


namespace ExaminationSystem.Features.GetAllDiplomas.Queries
{
    public record GetAllDiplomasQuery(int Page = 1, int PerPage = 20) : IRequest<RequestResult<GetAllDiplomaPaginatedDTO>>;

    public class GetAllDiplomasQueryValidator : AbstractValidator<GetAllDiplomasQuery>
    {
        public GetAllDiplomasQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page number must be greater than 0");
            RuleFor(x => x.PerPage)
                .GreaterThan(0).WithMessage("PerPage must be greater than 0");
        }
    }

    public class GetAllDiplomasQueryHandler : IRequestHandler<GetAllDiplomasQuery, RequestResult<GetAllDiplomaPaginatedDTO>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IValidator<GetAllDiplomasQuery> _validator;

        public GetAllDiplomasQueryHandler(IGeneralRepository<Diploma> diplomaRepository, IValidator<GetAllDiplomasQuery> validator)
        {
            _diplomaRepository = diplomaRepository;
            _validator = validator;
        }
        public async Task<RequestResult<GetAllDiplomaPaginatedDTO>> Handle(GetAllDiplomasQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
              .ValidateRequestAsync<GetAllDiplomasQuery, GetAllDiplomaPaginatedDTO>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var diplomas = _diplomaRepository
                                                    .Get(d => d.Status == DiplomaStatus.Published)
                                                    .Select(d => new GetPublishedDiplomaResponseDTO(
                                                          d.Id,
                                                          d.Title,
                                                          d.Description,
                                                          d.Status,
                                                          d.Quizzes.Count(q => !q.isDeleted)
                                                     ));

            if (!await diplomas.AnyAsync())
            {
                return RequestResult<GetAllDiplomaPaginatedDTO>.Success(
                    new GetAllDiplomaPaginatedDTO([], request.Page, request.PerPage, 0, 0),
                    "No diplomas found",
                    RequestErrorCode.Success
                );
            }

            var (data, total, totalPages) = await diplomas
              .ToPaginatedAsync(request.Page, request.PerPage, cancellationToken);

            var responseDTOs = new GetAllDiplomaPaginatedDTO(
                data,
                request.Page,
                request.PerPage,
                total,
                totalPages
                );

            return RequestResult<GetAllDiplomaPaginatedDTO>.Success(responseDTOs, "Diplomas retrieved successfully", RequestErrorCode.Success);
        }

    }
}
