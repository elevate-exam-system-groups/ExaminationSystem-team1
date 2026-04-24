namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Commands.CreateQuestion
{
    public record CreateQuestionCommand(
    Guid QuizId,
    string Text,
    string? Explanation,
    int OrderIndex)
    : IRequest<RequestResult<CreateQuestionResponseDto>>;
}
