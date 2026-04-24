namespace ExaminationSystem.Features.DiplomaModule.GetDiplomaWithQuizzesForLoggedStudent.Requests.GetQuizSummaryForStudent.DTOS
{
    public record GetQuizSummaryDTO(
      int AttemptCount,
      bool CanAttempt,
      decimal? LastScore
  );
}
