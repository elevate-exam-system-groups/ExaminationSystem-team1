using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.DiplomaModule.Shared.Orchestrators.GetPuplishedDiplomaByIdWithQuizzesOrchestrator.DTOS
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
