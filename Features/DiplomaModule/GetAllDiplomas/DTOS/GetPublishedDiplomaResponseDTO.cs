namespace ExaminationSystem.Features.DiplomaModule.GetAllDiplomas.DTOS
{
    public record GetPublishedDiplomaResponseDTO(Guid Id, string Title, string? Description, DiplomaStatus Status, int QuizCount);

}
