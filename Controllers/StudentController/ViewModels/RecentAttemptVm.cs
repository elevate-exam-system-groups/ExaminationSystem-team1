namespace ExaminationSystem.Controllers.StudentController.ViewModels
{
    public record RecentAttemptVm(
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
