namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Queries
{
    public record GetNextQuestionOrderQuery(Guid QuizId)
        : IRequest<RequestResult<NextQuestionOrderDto>>;
}
