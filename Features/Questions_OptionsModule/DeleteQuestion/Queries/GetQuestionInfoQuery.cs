namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Queries
{
    public record GetQuestionInfoQuery(Guid QuestionId)
        : IRequest<RequestResult<QuestionInfoDto>>;

}
