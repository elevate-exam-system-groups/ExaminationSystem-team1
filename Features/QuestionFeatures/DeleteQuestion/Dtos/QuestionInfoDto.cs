namespace ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Dtos
{
    public record QuestionInfoDto(
         Guid QuizId,
         QuizStatus QuizStatus
     );
}
