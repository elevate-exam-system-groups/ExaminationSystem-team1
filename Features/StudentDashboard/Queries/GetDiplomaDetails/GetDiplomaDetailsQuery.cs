using ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetDiplomaDetails
{
    public record GetDiplomaDetailsQuery(List<Guid> DiplomaIds)
     : IRequest<RequestResult<Dictionary<Guid, DiplomaInfo>>>;
}
