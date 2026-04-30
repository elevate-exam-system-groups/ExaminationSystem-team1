using ExaminationSystem.Features.StudentDashboard.DTOs.Diploma;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetDiplomaDetails
{
    public class GetDiplomaDetailsQueryHandler
        : IRequestHandler<GetDiplomaDetailsQuery, RequestResult<DiplomaDetailsDto>>
    {

        private readonly IGeneralRepository<Diploma> _diplomaRepo;
        public GetDiplomaDetailsQueryHandler(IGeneralRepository<Diploma> diplomaRepo)
        {
             _diplomaRepo = diplomaRepo;
        }

        public async Task<RequestResult<DiplomaDetailsDto>> Handle(
            GetDiplomaDetailsQuery request, CancellationToken ct)
        {

            //if (!request.DiplomaIds.Any())  //////////////////=> generic reposatory
            //    return RequestResult<DiplomaDetailsDto>.Success(
            //        new DiplomaDetailsDto(new()));

            var diplomas = await _diplomaRepo
                .Get(d => request.DiplomaIds.Contains(d.Id) && !d.IsDeleted)
                .ToDictionaryAsync(
                    k => k.Id,
                    v => new DiplomaInfoDTO(v.Title, v.Description),
                    ct);

            return RequestResult<DiplomaDetailsDto>.Success(
                   new DiplomaDetailsDto(diplomas));
        }
    }
}

