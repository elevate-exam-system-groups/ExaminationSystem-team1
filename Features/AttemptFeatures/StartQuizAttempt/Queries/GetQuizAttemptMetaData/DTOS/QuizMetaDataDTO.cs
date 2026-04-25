namespace ExaminationSystem.Features.AttemptFeatures.StartQuizAttempt.Queries.GetQuizAttemptMetaData.DTOS
{
    public record QuizMetaDataDTO
    (
      Guid DiplomaId,
      string QuizTitle,
      string? QuizInstructions,
      decimal PassScore,
      List<QuestionDTO> Questions
    );
}
