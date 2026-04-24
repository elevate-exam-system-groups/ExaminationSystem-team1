using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.DeleteQuestion.Queries.CheckActiveAttempts
{
    public record CheckActiveAttemptsQuery(Guid QuizId)
     : IRequest<RequestResult<ActiveAttemptsDto>>;
}
