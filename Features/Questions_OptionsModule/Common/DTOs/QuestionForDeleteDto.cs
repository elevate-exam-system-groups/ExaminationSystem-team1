namespace ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs
{
    public record QuestionForDeleteDto(
       Guid QuestionId,
       Guid QuizId,
       QuizStatus QuizStatus
   );

}
