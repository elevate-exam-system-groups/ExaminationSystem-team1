using ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Dtos;

namespace ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Queries.QuestionUpdateStatus
{
    public record GetQuestionUpdateStatusQuery(Guid QuestionId)
    : IRequest<RequestResult<QuestionUpdateStatusDto>>;
}
