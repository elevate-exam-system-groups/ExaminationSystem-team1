namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Queries.CheckActiveAttempts
{
    public record CheckActiveAttemptsQuery(Guid QuizId)
     : IRequest<RequestResult<ActiveAttemptsDto>>;
}
