namespace ExaminationSystem.Controllers.StudentController.ViewModels
{
    public record EnrolledDiplomaVm(
        Guid DiplomaId,
        string Title,
        string? Description,
        int TotalQuizzes,
        int CompletedQuizzes,
        decimal ProgressPercentage
    );
}
