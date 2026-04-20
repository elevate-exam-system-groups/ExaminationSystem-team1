namespace ExaminationSystem.Features.StudentDashboard.DTOs
{
    public record TotalQuizzesCountDto(Dictionary<Guid, int> CountByDiplomaId);
}
