using ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Commands.DeleteOptions
{
    public record DeleteOptionsCommand(Guid QuestionId)
        : IRequest<RequestResult<DeleteResponseDto>>;
}
