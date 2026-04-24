namespace ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzes.Requests.GetDiplomaQuizzesForLoggedStudent.DTOS
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
