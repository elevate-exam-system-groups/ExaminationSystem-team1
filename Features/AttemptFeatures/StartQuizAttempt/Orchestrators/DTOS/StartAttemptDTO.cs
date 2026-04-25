using ExaminationSystem.Features.AttemptFeatures.StartQuizAttempt.Queries.GetQuizAttemptMetaData.DTOS;

namespace ExaminationSystem.Features.AttemptFeatures.StartQuizAttempt.Orchestrators.DTOS
{
    public record StartAttemptDTO
    (
        Guid AttemptId,
        QuizMetaDataDTO metaData
    );
}
