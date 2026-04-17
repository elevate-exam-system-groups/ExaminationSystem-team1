namespace ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs
{
    public record QuestionForEditDto(
     Guid QuestionId,
     string Text,
     string? Explanation,
     QuizStatus QuizStatus
    );

}
