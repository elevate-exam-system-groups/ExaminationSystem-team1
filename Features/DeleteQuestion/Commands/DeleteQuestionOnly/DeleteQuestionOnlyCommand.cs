using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.DeleteQuestion.Commands.DeleteQuestionOnly
{
    public record DeleteQuestionOnlyCommand(Guid QuestionId)
    : IRequest<RequestResult<DeleteResponseDto>>;
}
