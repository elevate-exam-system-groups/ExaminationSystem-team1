namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Commands
{
    public record UpdateOptionsCommand(Guid QuestionId, List<UpdateOptionDto> Options)
    : IRequest<RequestResult<UpdateQuestionResponse>>;
}
