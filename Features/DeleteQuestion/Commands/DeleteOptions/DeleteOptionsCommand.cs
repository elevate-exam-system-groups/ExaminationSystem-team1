using ExaminationSystem.Features.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.DeleteQuestion.Commands.DeleteOptions
{
    public record DeleteOptionsCommand(Guid QuestionId)
        : IRequest<RequestResult<DeleteResponseDto>>;
}
