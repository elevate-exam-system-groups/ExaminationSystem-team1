using ExaminationSystem.Features.AdminDashboard.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.AdminDashboard.Queries.GetAttemptsAvgPassRate
{
    public record GetAttemptsAvgPassRateQuery : IRequest<RequestResult<AttemptAvgPassRateDto>>;
}
