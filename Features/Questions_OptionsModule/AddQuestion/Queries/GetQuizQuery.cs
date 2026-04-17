namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Queries
{
    public record GetQuizQuery(Guid quizId): IRequest<RequestResult<QuizDto>>;
}
