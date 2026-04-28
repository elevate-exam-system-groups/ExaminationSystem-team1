using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Queries.GetAttemptsOverTime
{
    public record GetAttemptsOverTimeQuery(DateTime? From = null, DateTime? To = null)
        : IRequest<RequestResult<List<AttemptsOverTimeDto>>>;
}
