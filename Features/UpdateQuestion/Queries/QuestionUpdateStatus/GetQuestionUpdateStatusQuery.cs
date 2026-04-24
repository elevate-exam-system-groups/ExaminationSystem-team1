using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.UpdateQuestion.Dtos;

namespace ExaminationSystem.Features.UpdateQuestion.Queries.QuestionUpdateStatus
{
    public record GetQuestionUpdateStatusQuery(Guid QuestionId)
    : IRequest<RequestResult<QuestionUpdateStatusDto>>;
}
