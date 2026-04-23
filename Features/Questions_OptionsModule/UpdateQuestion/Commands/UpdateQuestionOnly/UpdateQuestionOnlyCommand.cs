namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Commands.UpdateQuestionOnly
{
    public record UpdateQuestionOnlyCommand(Guid QuestionId, string Text, string? Explanation) :
        IRequest<RequestResult<UpdateQuestionOnlyResponse>>;

}
