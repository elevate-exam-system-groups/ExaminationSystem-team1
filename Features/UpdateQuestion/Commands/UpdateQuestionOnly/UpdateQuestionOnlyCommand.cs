using ExaminationSystem.Features.UpdateQuestion.Dtos;

namespace ExaminationSystem.Features.UpdateQuestion.Commands.UpdateQuestionOnly
{
    public record UpdateQuestionOnlyCommand(Guid QuestionId, string Text, string? Explanation) :
        IRequest<RequestResult<UpdateQuestionOnlyResponseDto>>;

}
