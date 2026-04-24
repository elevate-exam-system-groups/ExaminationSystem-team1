using ExaminationSystem.Features.AddQuestion.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.AddQuestion.Queries.GetNextQuestionOrder
{
    public record CalculateNextQuestionOrderQuery(Guid QuizId)
        : IRequest<RequestResult<NextQuestionOrderDto>>;
}
