using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Queries.GetPassRateByQuizQuery
{
    public record GetPassRateByQuizQuery(DateTime? From = null, DateTime? To = null)
    : IRequest<RequestResult<List<QuizPassRateDto>>>;
}
