using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.DiplomaModule.GetPublishedDiplomaRecord.DTOS
{
    public record GetPublishedDiplomaByIDResponseDTO([Required] Guid Id, string Title, string? Description, DiplomaStatus Status, int Count);

}
