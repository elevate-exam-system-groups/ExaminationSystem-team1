namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Queries.GetQuestionInfo
{
    public record GetQuestionInfoQuery(Guid QuestionId)
        : IRequest<RequestResult<QuestionInfoDto>>;
}
