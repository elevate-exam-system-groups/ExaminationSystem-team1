using ExaminationSystem.Features.DiplomaModule.GetAllDiplomas.DTOS;


namespace ExaminationSystem.Features.DiplomaModule.GetAllDiplomas.Request
{
    public record GetDiplomasQueryRequest(int Page = 1, int PerPage = 20) : IRequest<RequestResult<GetAllDiplomaPaginatedDTO>>;

    public class GetDiplomasQueryHandler : IRequestHandler<GetDiplomasQueryRequest, RequestResult<GetAllDiplomaPaginatedDTO>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;

        public GetDiplomasQueryHandler(IGeneralRepository<Diploma> diplomaRepository)
        {
            _diplomaRepository = diplomaRepository;
        }
        public async Task<RequestResult<GetAllDiplomaPaginatedDTO>> Handle(GetDiplomasQueryRequest request, CancellationToken cancellationToken)
        {

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

            var total = await diplomas.CountAsync();
            var totalPages = (int)Math.Ceiling(total / (double)request.PerPage);
            var data = await diplomas
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .ToListAsync();

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
