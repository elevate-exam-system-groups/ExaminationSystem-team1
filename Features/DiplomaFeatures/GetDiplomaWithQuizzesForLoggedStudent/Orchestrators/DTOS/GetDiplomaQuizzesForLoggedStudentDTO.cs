namespace ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent.Orchestrators.DTOS
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
