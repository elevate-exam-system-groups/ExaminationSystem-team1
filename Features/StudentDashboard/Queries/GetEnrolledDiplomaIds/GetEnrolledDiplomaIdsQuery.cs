using ExaminationSystem.Features.StudentDashboard.DTOs.Diploma;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetEnrolledDiplomaIds
{
    public record GetEnrolledDiplomaIdsQuery(string StudentId)
     : IRequest<RequestResult<EnrolledDiplomaIdsDto>>;
}
