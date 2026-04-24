using ExaminationSystem.Features.AddQuestion.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.AddQuestion.Queries.CheckQuizStatus
{
    public record CheckQuizStatusQuery(Guid quizId) : IRequest<RequestResult<QuizStatusDto>>;
}
