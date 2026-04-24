namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Queries.GetQuestionForEdit
{
    public record GetQuestionUpdateStatusQuery(Guid QuestionId)
    : IRequest<RequestResult<QuestionUpdateStatusDto>>;
}
