using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.Common.Command.Request
{
    public record DeleteOptionsCommand(Guid QuestionId)
        : IRequest<RequestResult<DeleteOptionsResponse>>;
}
