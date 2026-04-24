using ExaminationSystem.Features.AddQuestion.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.AddQuestion.Commands.CreateQuestion
{
    public record CreateQuestionCommand(
    Guid QuizId,
    string Text,
    string? Explanation,
    int OrderIndex)
    : IRequest<RequestResult<CreateQuestionResponseDto>>;
}
