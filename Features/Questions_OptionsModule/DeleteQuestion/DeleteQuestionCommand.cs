using ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion
{
    public record DeleteQuestionCommand(Guid Id) : IRequest<RequestResult<DeleteQuestionResponse>>;
}
