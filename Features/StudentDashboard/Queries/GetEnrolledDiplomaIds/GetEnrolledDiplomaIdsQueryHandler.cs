namespace ExaminationSystem.Features.StudentDashboard.Queries.GetEnrolledDiplomaIds
{
    public class GetEnrolledDiplomaIdsQueryHandler
     : IRequestHandler<GetEnrolledDiplomaIdsQuery, RequestResult<List<Guid>>>
    {

        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;
        public GetEnrolledDiplomaIdsQueryHandler(IGeneralRepository<Enrollment> enrollmentRepo)
            => _enrollmentRepo = enrollmentRepo;

        public async Task<RequestResult<List<Guid>>> Handle(
            GetEnrolledDiplomaIdsQuery request, CancellationToken ct)
        {
            var diplomaIds = await _enrollmentRepo
                .Get(e => e.StudentId == request.StudentId && !e.isDeleted)
                .Select(e => e.DiplomaId)
                .ToListAsync(ct);

            return RequestResult<List<Guid>>.Success(diplomaIds);
        }
    }
}