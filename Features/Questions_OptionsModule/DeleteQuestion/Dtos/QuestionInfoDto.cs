namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Dtos
{
    public record QuestionInfoDto(
         Guid QuestionId,
         Guid QuizId,
         QuizStatus QuizStatus
     );
}
