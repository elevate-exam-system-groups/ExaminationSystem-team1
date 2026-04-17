namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Queries
{
    public record CheckActiveAttemptsQuery(Guid QuizId)
     : IRequest<RequestResult<ActiveAttemptsDto>>;
}
