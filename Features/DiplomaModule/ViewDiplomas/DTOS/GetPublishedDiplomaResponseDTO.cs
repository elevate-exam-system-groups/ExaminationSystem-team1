namespace ExaminationSystem.Features.DiplomaModule.ViewDiplomas.DTOS
{
    public record GetPublishedDiplomaResponseDTO(Guid Id, string Title, string? Description, DiplomaStatus Status, int QuizCount);

}
