namespace ExaminationSystem.Features.StudentDashboard.DTOs.Diploma
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
