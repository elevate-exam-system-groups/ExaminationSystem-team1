using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.Common.DiplomaRequests.Orchestrators.GetPuplishedDiplomaByIdWithQuizzesOrchestrator.DTOS
{
    public record GetPuplishedDiplomaQuizzesByIdDTO
        (
        [Required] Guid Id,
        string Title,
        string? Description,
        DiplomaStatus Status,
        int QuizzesCount
        );


}
