using ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Queries.GetQuestionInfo
{
    public record GetQuestionInfoQuery(Guid QuestionId)
        : IRequest<RequestResult<QuestionInfoDto>>;
}
