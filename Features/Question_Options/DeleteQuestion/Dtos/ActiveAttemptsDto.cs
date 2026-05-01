namespace ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Dtos
{
    public record ActiveAttemptsDto(
     Guid QuizId,
     bool HasActiveAttempts,
     int ActiveAttemptsCount
 );
}
