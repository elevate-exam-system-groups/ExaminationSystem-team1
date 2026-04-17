namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Commands
{
    public record DeleteOptionsCommand(Guid QuestionId)
        : IRequest<RequestResult<DeleteResponse>>;
}
