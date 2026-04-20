namespace ExaminationSystem.Features.StudentDashboard.DTOs
{
    public record EnrolledDiplomaDto(
       Guid DiplomaId,
       string Title,
       string? Description,
       int TotalQuizzes,
       int CompletedQuizzes,
       decimal ProgressPercentage
    );
}
