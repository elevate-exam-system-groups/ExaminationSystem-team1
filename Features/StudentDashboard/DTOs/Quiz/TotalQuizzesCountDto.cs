namespace ExaminationSystem.Features.StudentDashboard.DTOs.Quiz
{
    public record TotalQuizzesCountDto(Dictionary<Guid, int> CountByDiplomaId);
}
