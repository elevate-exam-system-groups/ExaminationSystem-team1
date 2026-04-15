namespace ExaminationSystem.Controllers.DiplomaController.ViewModels
{
    public record CreateDiplomaResponseVM(Guid Id, string Title, string? Description, DiplomaStatus Status);
    public record CreateDiplomaRequestVM(string Title, string? Description);


}
