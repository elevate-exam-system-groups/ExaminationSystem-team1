using ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.DTOs;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Queries.GetAttemptDetail
{
    public record GetAttemptDetailQuery(Guid AttemptId)
     : IRequest<RequestResult<AttemptDetailDto>>;
}
