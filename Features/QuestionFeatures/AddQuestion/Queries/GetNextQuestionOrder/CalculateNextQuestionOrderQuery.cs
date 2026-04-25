using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.QuestionFeatures.AddQuestion.DTOs;

namespace ExaminationSystem.Features.QuestionFeatures.AddQuestion.Queries.GetNextQuestionOrder
{
    public record CalculateNextQuestionOrderQuery(Guid QuizId)
        : IRequest<RequestResult<NextQuestionOrderDto>>;
}
