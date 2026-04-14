namespace ExaminationSystem.Controllers.DiplomaController.ViewModels
{
    public record UpdateDiplomaResponseVM(Guid Id, string Title, string? Description, DiplomaStatus Status);
    public record UpdateDiplomaRequestVM(Guid Id, string? Title, string? Description);
}
