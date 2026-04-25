namespace ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent.Queries.GetQuizSummaryForStudent.DTOS
{
    public record GetQuizSummaryDTO(
      int AttemptCount,
      bool CanAttempt,
      decimal? LastScore
  );
}
