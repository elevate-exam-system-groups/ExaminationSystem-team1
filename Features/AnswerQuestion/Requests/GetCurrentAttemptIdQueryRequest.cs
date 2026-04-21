namespace ExaminationSystem.Features.AnswerQuestion.Requests
{
    public record GetCurrentAttemptIdQueryRequest(Guid quizID, string StudentId)
        : IRequest<RequestResult<Guid>>;

}
