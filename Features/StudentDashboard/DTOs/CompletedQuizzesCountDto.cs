namespace ExaminationSystem.Features.StudentDashboard.DTOs
{
    public record CompletedQuizzesCountDto(Dictionary<Guid, int> CountByDiplomaId);
}
