namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Commands
{
    public record DeleteQuestionOnlyCommand(Guid QuestionId)
    : IRequest<RequestResult<DeleteResponse>>;
}
