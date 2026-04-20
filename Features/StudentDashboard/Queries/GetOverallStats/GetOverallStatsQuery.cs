using ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetOverallStats
{
    public record GetOverallStatsQuery(string StudentId)
     : IRequest<RequestResult<OverallStatsDto>>;
}
