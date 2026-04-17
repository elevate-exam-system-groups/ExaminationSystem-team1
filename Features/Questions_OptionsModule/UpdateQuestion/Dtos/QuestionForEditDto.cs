namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Dtos
{
    public record QuestionForEditDto(
     Guid QuestionId,
     string Text,
     string? Explanation,
     QuizStatus QuizStatus
    );

}
