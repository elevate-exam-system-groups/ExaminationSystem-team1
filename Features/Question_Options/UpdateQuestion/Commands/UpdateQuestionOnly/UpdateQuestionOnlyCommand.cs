using ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Dtos;

namespace ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Commands.UpdateQuestionOnly
{
    public record UpdateQuestionOnlyCommand(Guid QuestionId, string Text, string? Explanation) :
        IRequest<RequestResult<UpdateQuestionOnlyResponseDto>>;

}
