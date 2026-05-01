using ExaminationSystem.Features.AdminDashboard.DTOs;

namespace ExaminationSystem.Features.AdminDashboard.Queries.GetAttemptsAvgPassRate
{
    public record GetAttemptsAvgPassRateQuery() : IRequest<RequestResult<AttemptAvgPassRateDto>>;
}
