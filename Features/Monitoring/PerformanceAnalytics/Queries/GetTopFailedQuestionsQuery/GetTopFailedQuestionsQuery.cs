using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Queries.GetTopFailedQuestionsQuery
{
    public record GetTopFailedQuestionsQuery(DateTime? From = null, DateTime? To = null)
   : IRequest<RequestResult<List<FailedQuestionDto>>>;
}
