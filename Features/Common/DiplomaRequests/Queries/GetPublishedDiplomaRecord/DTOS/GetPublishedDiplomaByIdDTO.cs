using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.Common.DiplomaRequests.Queries.GetPublishedDiplomaRecord.DTOS
{
    public record GetPublishedDiplomaByIdDTO(
        [Required] Guid Id,
        string Title,
        string? Description,
        DiplomaStatus Status
        );

}
