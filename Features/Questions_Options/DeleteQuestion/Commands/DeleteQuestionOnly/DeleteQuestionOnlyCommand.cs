namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Commands.DeleteQuestionOnly
{
    public record DeleteQuestionOnlyCommand(Guid QuestionId)
    : IRequest<RequestResult<DeleteResponseDto>>;
}
