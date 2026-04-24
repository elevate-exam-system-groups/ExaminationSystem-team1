using ExaminationSystem.Features.StartQuizAttempt.Queries.GetQuizAttemptMetaData.DTOS;

namespace ExaminationSystem.Features.StartQuizAttempt.Orchestrators.DTOS
{
    public record StartAttemptDTO
    (
        Guid AttemptId,
        QuizMetaDataDTO metaData
    );
}
