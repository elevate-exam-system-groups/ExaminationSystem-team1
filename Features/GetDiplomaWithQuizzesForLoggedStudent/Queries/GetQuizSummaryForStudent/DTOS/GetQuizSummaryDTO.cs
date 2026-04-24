namespace ExaminationSystem.Features.GetDiplomaWithQuizzesForLoggedStudent.Queries.GetQuizSummaryForStudent.DTOS
{
    public record GetQuizSummaryDTO(
      int AttemptCount,
      bool CanAttempt,
      decimal? LastScore
  );
}
