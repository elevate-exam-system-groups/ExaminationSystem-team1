namespace ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzesForSignedInStudent.DTOS
{
    public record ViewDiplomaQuizzesResponseDTO
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
