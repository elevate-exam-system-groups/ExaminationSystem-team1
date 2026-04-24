namespace ExaminationSystem.Features.DeleteQuestion.Dtos
{
    public record QuestionInfoDto(
         Guid QuizId,
         QuizStatus QuizStatus
     );
}
