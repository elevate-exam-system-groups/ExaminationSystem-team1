namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Commands.DeleteOptions
{
    public record DeleteOptionsCommand(Guid QuestionId)
        : IRequest<RequestResult<DeleteResponseDto>>;
}
