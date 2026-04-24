namespace ExaminationSystem.Features.SubmitQuizAttempt.Orchestrators.DTOS
{
    public record SubmitAttemptResultDto
        (
        decimal Score,
        bool IsPassed,
        QuizAttemptStatus Status
        );

}
