namespace ExaminationSystem.Features.DiplomaFeatures.GetAllDiplomas.Queries.DTOS
{
    public record GetPublishedDiplomaResponseDTO(Guid Id, string Title, string? Description, DiplomaStatus Status, int QuizCount);

}
