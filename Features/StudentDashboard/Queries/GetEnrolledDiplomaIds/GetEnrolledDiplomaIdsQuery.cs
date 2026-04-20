using ExaminationSystem.Features.StudentDashboard.DTOs.ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetEnrolledDiplomaIds
{
    public record GetEnrolledDiplomaIdsQuery(string StudentId)
     : IRequest<RequestResult<StudentEnrolledIdsDto>>;

}
