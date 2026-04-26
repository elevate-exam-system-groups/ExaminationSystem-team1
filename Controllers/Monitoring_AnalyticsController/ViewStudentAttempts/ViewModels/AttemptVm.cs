namespace ExaminationSystem.Controllers.Monitoring_Analytics.ViewStudentAttempts.ViewModels
{
    public record AttemptVm(
         Guid AttemptId,
         string StudentId,
         string StudentName,
         string StudentEmail,
         Guid QuizId,
         string QuizTitle,
         string Status,
         decimal? Score,
         bool? IsPassed,
         DateTime StartTime,
         DateTime? SubmittedAt
   );
}
