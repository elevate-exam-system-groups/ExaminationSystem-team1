using ExaminationSystem.Features.Attempts.StartQuizAttempt.Queries.GetQuizAttemptMetaData.DTOS;

namespace ExaminationSystem.Features.Attempts.StartQuizAttempt.Orchestrators.DTOS
{
    public record StartAttemptDTO
    (
        Guid AttemptId,
        AttemptMetaDataDTO metaData
    );
}
