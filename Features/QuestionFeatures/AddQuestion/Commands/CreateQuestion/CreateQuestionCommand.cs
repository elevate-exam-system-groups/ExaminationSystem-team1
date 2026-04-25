using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.QuestionFeatures.AddQuestion.DTOs;

namespace ExaminationSystem.Features.QuestionFeatures.AddQuestion.Commands.CreateQuestion
{
    public record CreateQuestionCommand(
    Guid QuizId,
    string Text,
    string? Explanation,
    int OrderIndex)
    : IRequest<RequestResult<CreateQuestionResponseDto>>;
}
