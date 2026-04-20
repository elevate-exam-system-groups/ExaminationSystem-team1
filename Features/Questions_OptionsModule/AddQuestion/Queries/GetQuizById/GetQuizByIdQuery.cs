namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Queries.GetQuiz
{
    public record GetQuizByIdQuery(Guid quizId) : IRequest<RequestResult<QuizDto>>;
}
