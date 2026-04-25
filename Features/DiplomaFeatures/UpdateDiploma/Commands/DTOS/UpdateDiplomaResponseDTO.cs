namespace ExaminationSystem.Features.DiplomaFeatures.UpdateDiploma.Commands.DTOS
{
    public record UpdateDiplomaResponseDTO(Guid Id, string Title, string? Description, DiplomaStatus Status);

}
