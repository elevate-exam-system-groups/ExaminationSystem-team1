using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Queries.GetAttemptsAvgPassRate
{
    public record GetAttemptsAvgPassRateQuery : IRequest<RequestResult<AttemptAvgPassRateDto>>;
}
