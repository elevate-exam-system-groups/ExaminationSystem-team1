using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.DiplomaFeatures.SharedDiploma.Queries.GetPublishedDiplomaRecord.DTOS
{
    public record GetPublishedDiplomaByIdDTO(
        [Required] Guid Id,
        string Title,
        string? Description,
        DiplomaStatus Status
        );

}
