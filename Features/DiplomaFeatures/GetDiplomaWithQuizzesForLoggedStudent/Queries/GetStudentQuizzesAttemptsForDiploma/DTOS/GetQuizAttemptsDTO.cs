namespace ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent.Queries.GetStudentQuizzesAttemptsForDiploma.DTOS
{
    public class GetQuizAttemptsDTO
    (
     Guid AttemptId,
     Guid QuizId,
     decimal? AttemptScore,
     QuizAttemptStatus Status,
     DateTime? SubmittedAt,
     int AttemptCount

    );
}
