using ExaminationSystem.Features.Admin.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Admin.Queries.GetAttemptsAvgPassRate
{
    public record GetAttemptsAvgPassRateQuery : IRequest<RequestResult<AttemptAvgPassRateDto>>;
}
