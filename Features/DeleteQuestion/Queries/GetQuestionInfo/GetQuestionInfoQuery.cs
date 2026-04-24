using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.DeleteQuestion.Queries.GetQuestionInfo
{
    public record GetQuestionInfoQuery(Guid QuestionId)
        : IRequest<RequestResult<QuestionInfoDto>>;
}
