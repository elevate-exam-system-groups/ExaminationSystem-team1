using ExaminationSystem.Features.DiplomaModule.ViewDiplomas.DTOS;

namespace ExaminationSystem.Controllers.DiplomaController.ViewModels
{
    public record AllDiplomasPaginatedRequestVM(int Page = 1, int PerPage = 20);

    public record GetAllDiplomasPaginatedResponseVM
     (
     List<GetPublishedDiplomaResponseDTO> Data,
     int Page,
     int PerPage,
     int Total,
     int TotalPages
     );

    public record GetPublishedDiplomaResponseVM(Guid Id, string Title, string? Description, DiplomaStatus Status, int QuizCount);
}
