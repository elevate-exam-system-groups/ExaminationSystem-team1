using ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetDiplomaDetails
{
    public class GetDiplomaDetailsQueryHandler
        : IRequestHandler<GetDiplomaDetailsQuery, RequestResult<Dictionary<Guid, DiplomaInfo>>>
    {

        private readonly IGeneralRepository<Diploma> _diplomaRepo;
        public GetDiplomaDetailsQueryHandler(IGeneralRepository<Diploma> diplomaRepo)
            => _diplomaRepo = diplomaRepo;

        public async Task<RequestResult<Dictionary<Guid, DiplomaInfo>>> Handle(
            GetDiplomaDetailsQuery request, CancellationToken ct)
        {
            if (!request.DiplomaIds.Any())
                return RequestResult<Dictionary<Guid, DiplomaInfo>>.Success(new());

            var diplomas = await _diplomaRepo
                .Get(d => request.DiplomaIds.Contains(d.Id) && !d.isDeleted)
                .ToDictionaryAsync(
                    k => k.Id,
                    v => new DiplomaInfo(v.Title, v.Description),
                    ct);

            return RequestResult<Dictionary<Guid, DiplomaInfo>>.Success(diplomas);
        }
    }
}

