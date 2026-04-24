namespace ExaminationSystem.Features.StudentDashboard.DTOs.Quiz
{
    public record CompletedQuizzesCountDto(Dictionary<Guid, int> CountByDiplomaId);
}
