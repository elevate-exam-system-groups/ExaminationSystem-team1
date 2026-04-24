namespace ExaminationSystem.Features.GetDiplomaWithQuizzesForLoggedStudent.Queries.GetDiplomaQuizzes.DTOS
{
    public record GetDiplomaQuizzesDTO
    (
        Guid QuizId,
        string Title,
        int DurationInMinutes,
        decimal PassScore,
        int? MaxAttempts,
        QuizStatus Status
    );
}
