using ExaminationSystem.Features.StudentDashboard.DTOs.Diploma;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetDiplomaDetails
{
    public record GetDiplomaDetailsQuery(List<Guid> DiplomaIds)
     : IRequest<RequestResult<DiplomaDetailsDto>>;
}
