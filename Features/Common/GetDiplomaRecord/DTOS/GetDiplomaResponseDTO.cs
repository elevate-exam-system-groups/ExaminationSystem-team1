namespace ExaminationSystem.Features.Common.GetDiplomaRecord.DTOS
{
    public record GetDiplomaResponseDTO(Guid Id, string Title, string? Description, DiplomaStatus Status)
    {
    }
}
