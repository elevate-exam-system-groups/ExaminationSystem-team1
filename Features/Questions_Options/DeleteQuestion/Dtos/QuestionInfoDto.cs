namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Dtos
{
    public record QuestionInfoDto(
         Guid QuizId,
         QuizStatus QuizStatus
     );
}
