namespace ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs
{
    public record QuestionInfoDto(
         Guid QuestionId,
         Guid QuizId,
         QuizStatus QuizStatus
     );
}
