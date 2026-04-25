using ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Commands.DeleteQuestionOnly
{
    public record DeleteQuestionOnlyCommand(Guid QuestionId)
    : IRequest<RequestResult<DeleteResponseDto>>;
}
