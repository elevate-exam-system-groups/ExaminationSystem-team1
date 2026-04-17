using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.Common.Command.Request
{
    public record UpdateQuestionOnlyCommand(Guid QuestionId, string Text, string? Explanation) :
        IRequest<RequestResult<UpdateQuestionResponse>>;

}
