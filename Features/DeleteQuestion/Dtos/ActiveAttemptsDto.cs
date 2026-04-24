namespace ExaminationSystem.Features.DeleteQuestion.Dtos
{
    public record ActiveAttemptsDto(
     Guid QuizId,
     bool HasActiveAttempts,
     int ActiveAttemptsCount
 );
}
