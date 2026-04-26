namespace ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.DTOs
{
    public record AttemptDetailDto(
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
         DateTime? SubmittedAt,
         int DurationInMinutes,
         List<AttemptAnswerDto> Answers
    );
}