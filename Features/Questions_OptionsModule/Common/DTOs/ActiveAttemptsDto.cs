namespace ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs
{
    public record ActiveAttemptsDto(
     Guid QuizId,
     bool HasActiveAttempts,
     int ActiveAttemptsCount
 );
}
