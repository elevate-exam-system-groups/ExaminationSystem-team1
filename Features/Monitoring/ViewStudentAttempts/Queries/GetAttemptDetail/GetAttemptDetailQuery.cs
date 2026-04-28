using ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.DTOs;

namespace ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.Queries.GetAttemptDetail
{
    public record GetAttemptDetailQuery(Guid AttemptId)
     : IRequest<RequestResult<AttemptDetailDto>>;
}
