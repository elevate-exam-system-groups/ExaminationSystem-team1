namespace ExaminationSystem.Features.Questions_OptionsModule.QuestionQuizStatus
{
    public record GetQuestionQuizStatusQuery(Guid QuestionId) : IRequest<RequestResult<QuestionStatusResult>>;
}
