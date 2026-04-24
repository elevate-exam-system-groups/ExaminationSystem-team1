namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Queries
{
    public record CalculateNextQuestionOrderQuery(Guid QuizId)
        : IRequest<RequestResult<NextQuestionOrderDto>>;
}
