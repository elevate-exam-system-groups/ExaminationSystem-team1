using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Queries
{
    public record GetAttemptsAvgPassRateQuery : IRequest<RequestResult<AttemptAvgPassRateDto>>;
}
