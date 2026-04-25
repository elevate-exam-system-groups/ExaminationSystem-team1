namespace ExaminationSystem.Features.DiplomaFeatures.CreateDiploma.Commands.DTOS
{
    public record CreateDiplomaResponseDTO(Guid Id, string Title, string? Description, DiplomaStatus Status)
    {
    }


}
