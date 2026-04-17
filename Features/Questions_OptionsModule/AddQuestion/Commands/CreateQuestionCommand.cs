namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Commands
{
    public record CreateQuestionCommand(
    Guid QuizId,
    string Text,
    string? Explanation,
    int OrderIndex) : IRequest<RequestResult<CreateQuestionResponse>>;
}
