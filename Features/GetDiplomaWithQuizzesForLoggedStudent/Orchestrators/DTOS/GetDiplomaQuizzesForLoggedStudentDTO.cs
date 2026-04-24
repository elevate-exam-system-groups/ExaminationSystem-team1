namespace ExaminationSystem.Features.GetDiplomaWithQuizzesForLoggedStudent.Orchestrators.DTOS
{
    public record GetDiplomaQuizzesForLoggedStudentDTO
    (
        Guid QuizId,
        string QuizTitle,
        int AttemptsCount,
        int DurationInMinutes,
        decimal passScore,
        int? maxAttempts,
        bool canAttempt,
        decimal? lastScore,
        QuizStatus Status
    );
}
