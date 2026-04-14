namespace ExaminationSystem.Features.Questions_OptionsModule.CreateOptions
{
    public record CreateOptionsCommand(Guid QuestionId , List<OptionDto> Options) 
        : IRequest<RequestResult<CreateOptionsResponse>>;
}
