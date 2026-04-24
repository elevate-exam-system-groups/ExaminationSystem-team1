namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Commands.CreateOptionsForQuestion
{
    public record CreateOptionsForQuestionCommand(Guid QuestionId, List<OptionDto> Options)
        : IRequest<RequestResult<CreateOptionsResponseDto>>;
}
