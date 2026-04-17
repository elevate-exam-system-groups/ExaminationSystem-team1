using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.DiplomaModule.GetDiplomaRecord.DTOS
{
    public record GetDiplomaByIDResponseDTO([Required] Guid Id, string Title, string? Description, DiplomaStatus Status, int Count);

}
