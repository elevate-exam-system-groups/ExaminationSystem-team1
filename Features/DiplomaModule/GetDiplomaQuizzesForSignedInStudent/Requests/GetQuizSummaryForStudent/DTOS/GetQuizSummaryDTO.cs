namespace ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzesForSignedInStudent.Requests.GetQuizSummaryForStudent.DTOS
{
    public record GetQuizSummaryDTO(
      int AttemptCount,
      bool CanAttempt,
      decimal? LastScore
  );
}
