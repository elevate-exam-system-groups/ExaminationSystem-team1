namespace ExaminationSystem.Features.StartQuizAttempt.Queries.GetQuizAttemptMetaData.DTOS
{
    public record QuestionDTO
    (
        Guid QuestionId,
        string Text,
        int OrderIndex,
        List<OptionDTO> Options
    );
}
