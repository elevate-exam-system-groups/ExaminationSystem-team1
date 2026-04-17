namespace ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs
{
    public record QuestionDeleteValidationDto(
      Guid QuestionId,
      Guid QuizId,
      QuizStatus QuizStatus,
      bool HasActiveAttempts
  );
}
