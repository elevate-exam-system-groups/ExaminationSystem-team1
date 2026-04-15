namespace ExaminationSystem.Features.DiplomaModule.CreateDiploma.DTOS
{
    public record CreateDiplomaResponseDTO(Guid Id, string Title, string? Description, DiplomaStatus Status)
    {
    }


}
