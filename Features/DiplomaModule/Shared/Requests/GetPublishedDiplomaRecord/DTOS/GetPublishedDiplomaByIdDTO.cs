using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.DiplomaModule.Shared.Requests.GetPublishedDiplomaRecord.DTOS
{
    public record GetPublishedDiplomaByIdDTO(
        [Required] Guid Id,
        string Title,
        string? Description,
        DiplomaStatus Status
        );

}
