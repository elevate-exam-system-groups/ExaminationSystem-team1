namespace ExaminationSystem.Features.DiplomaModule.UpdateDiploma.DTOS
{
    public record UpdateDiplomaResponseDTO(Guid Id, string Title, string? Description, DiplomaStatus Status)
    {
    }
}
