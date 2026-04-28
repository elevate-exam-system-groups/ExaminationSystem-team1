using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Queries.GetAvgScoreByDiploma
{
    public record GetAvgScoreByDiplomaQuery(DateTime? From = null, DateTime? To = null)
     : IRequest<RequestResult<List<DiplomaAvgScoreDto>>>;
}
