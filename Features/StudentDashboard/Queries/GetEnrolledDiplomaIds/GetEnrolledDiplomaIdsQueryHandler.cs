using ExaminationSystem.Features.StudentDashboard.DTOs.Diploma;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetEnrolledDiplomaIds
{
    public class GetEnrolledDiplomaIdsQueryHandler
     : IRequestHandler<GetEnrolledDiplomaIdsQuery, RequestResult<EnrolledDiplomaIdsDto>>
    {

        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;
        public GetEnrolledDiplomaIdsQueryHandler(IGeneralRepository<Enrollment> enrollmentRepo)
            => _enrollmentRepo = enrollmentRepo;

        public async Task<RequestResult<EnrolledDiplomaIdsDto>> Handle(
            GetEnrolledDiplomaIdsQuery request, CancellationToken ct)
        {

            var diplomaIds = await _enrollmentRepo
                .Get(e => e.StudentId == request.StudentId && !e.isDeleted)
                .Select(e => e.DiplomaId)
                .ToListAsync(ct);

            return RequestResult<EnrolledDiplomaIdsDto>.Success(
                new EnrolledDiplomaIdsDto(diplomaIds));
        }
    }
}