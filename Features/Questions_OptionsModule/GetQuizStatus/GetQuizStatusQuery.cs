namespace ExaminationSystem.Features.Questions_OptionsModule.GetQuizStatus
{
    public record GetQuizStatusQuery(Guid QuizId) : IRequest<RequestResult<QuizStatus>>;

}
