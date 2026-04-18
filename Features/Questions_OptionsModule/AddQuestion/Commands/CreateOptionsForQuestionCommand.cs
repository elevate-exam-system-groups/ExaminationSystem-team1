namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Commands
{
    public record CreateOptionsForQuestionCommand(Guid QuestionId, List<OptionDto> Options) 
        : IRequest<RequestResult<bool>>;
}
