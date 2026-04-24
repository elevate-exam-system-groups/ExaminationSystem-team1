namespace ExaminationSystem.Features.StudentDashboard.DTOs.Attempt
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
