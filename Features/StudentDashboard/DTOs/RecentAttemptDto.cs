namespace ExaminationSystem.Features.StudentDashboard.DTOs
{
    public record RecentAttemptDto(
        Guid AttemptId,
        Guid QuizId,
        string QuizTitle,
        string DiplomaTitle,
        string Status,
        decimal? Score,
        bool? IsPassed,
        DateTime? SubmittedAt
    );

}
