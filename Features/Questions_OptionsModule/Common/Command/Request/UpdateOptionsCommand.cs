using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.Common.Command.Request
{
    public record UpdateOptionsCommand(Guid QuestionId, List<UpdateOptionDto> Options)
    : IRequest<RequestResult<UpdateQuestionResponse>>;
}
