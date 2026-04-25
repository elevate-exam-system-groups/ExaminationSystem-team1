using ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Dtos;

namespace ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Commands.UpdateOptions
{
    public record UpdateOptionsCommand(Guid QuestionId, List<UpdateOptionDto> Options)
    : IRequest<RequestResult<UpdateOptionResponse>>;
}
