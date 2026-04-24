namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Dtos
{
    public record ActiveAttemptsDto(
     Guid QuizId,
     bool HasActiveAttempts,
     int ActiveAttemptsCount
 );
}
