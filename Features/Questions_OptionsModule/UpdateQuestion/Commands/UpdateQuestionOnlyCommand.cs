namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Commands
{
    public record UpdateQuestionOnlyCommand(Guid QuestionId, string Text, string? Explanation) :
        IRequest<RequestResult<UpdateQuestionResponse>>;

}
