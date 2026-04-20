namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Queries.GetQuestionForEdit
{
    public record GetQuestionByIdQuery(Guid QuestionId)
    : IRequest<RequestResult<QuestionForEditDto>>;
}
