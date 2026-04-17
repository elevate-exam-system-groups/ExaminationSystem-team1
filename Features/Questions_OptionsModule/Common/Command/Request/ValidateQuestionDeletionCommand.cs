using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.Common.Command.Request
{
    public record ValidateQuestionDeletionCommand(Guid QuestionId)
       : IRequest<RequestResult<QuestionDeletionValidationDto>>;
}
