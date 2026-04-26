namespace ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.DTOs
{
    public record AttemptDto(
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