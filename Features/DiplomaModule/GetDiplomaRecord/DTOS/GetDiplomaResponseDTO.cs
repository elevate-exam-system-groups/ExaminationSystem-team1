namespace ExaminationSystem.Features.DiplomaModule.GetDiplomaRecord.DTOS
{
    public record GetDiplomaByIDResponseDTO(Guid Id, string Title, string? Description, DiplomaStatus Status, int Count);

}
