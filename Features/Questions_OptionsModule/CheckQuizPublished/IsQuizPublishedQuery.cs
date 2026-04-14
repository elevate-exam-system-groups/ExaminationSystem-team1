namespace ExaminationSystem.Features.Questions_OptionsModule.CheckQuizPublished
{
    public record IsQuizPublishedQuery(Guid QuizId) : IRequest<RequestResult<bool>>;
}
