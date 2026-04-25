using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.QuestionFeatures.AddQuestion.DTOs;

namespace ExaminationSystem.Features.QuestionFeatures.AddQuestion.Queries.CheckQuizStatus
{
    public record CheckQuizStatusQuery(Guid quizId) : IRequest<RequestResult<QuizStatusDto>>;
}
