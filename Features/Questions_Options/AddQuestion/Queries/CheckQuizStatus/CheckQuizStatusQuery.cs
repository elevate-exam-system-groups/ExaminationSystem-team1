namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Queries.GetQuiz
{
    public record CheckQuizStatusQuery(Guid quizId) : IRequest<RequestResult<QuizStatusDto>>;
}
