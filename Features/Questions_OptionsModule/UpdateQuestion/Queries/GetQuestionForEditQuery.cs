namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Query
{
    public record GetQuestionForEditQuery(Guid QuestionId)
    : IRequest<RequestResult<QuestionForEditDto>>;
}
