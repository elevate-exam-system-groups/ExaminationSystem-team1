using ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Queries.CheckActiveAttempts
{
    public record CheckActiveAttemptsQuery(Guid QuizId)
     : IRequest<RequestResult<ActiveAttemptsDto>>;
}
