using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.StudentDashboard.DTOs;
using ExaminationSystem.Features.StudentDashboard.DTOs.Diploma;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetDiplomaDetails
{
    public class GetDiplomaDetailsQueryHandler
        : IRequestHandler<GetDiplomaDetailsQuery, RequestResult<DiplomaDetailsDto>>
    {

        private readonly IGeneralRepository<Diploma> _diplomaRepo;
        public GetDiplomaDetailsQueryHandler(IGeneralRepository<Diploma> diplomaRepo)
            => _diplomaRepo = diplomaRepo;

        public async Task<RequestResult<DiplomaDetailsDto>> Handle(
            GetDiplomaDetailsQuery request, CancellationToken ct)
        {
            if (!request.DiplomaIds.Any())
                return RequestResult<DiplomaDetailsDto>.Success(
                    new DiplomaDetailsDto(new()));

            var diplomas = await _diplomaRepo
                .Get(d => request.DiplomaIds.Contains(d.Id) && !d.isDeleted)
                .ToDictionaryAsync(
                    k => k.Id,
                    v => new DiplomaInfoDTO(v.Title, v.Description),
                    ct);

            return RequestResult<DiplomaDetailsDto>.Success(
                   new DiplomaDetailsDto(diplomas));
        }
    }
}

