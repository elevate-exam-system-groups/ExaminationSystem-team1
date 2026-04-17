namespace ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs
{
    public record QuestionDeletionValidationDto(
          Guid QuestionId,
          Guid QuizId,
          bool CanDelete,
          string? BlockReason
      );
}
