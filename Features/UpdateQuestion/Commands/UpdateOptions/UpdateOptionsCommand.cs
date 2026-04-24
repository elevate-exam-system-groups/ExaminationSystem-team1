using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.UpdateQuestion.Dtos;

namespace ExaminationSystem.Features.UpdateQuestion.Commands.UpdateOptions
{
    public record UpdateOptionsCommand(Guid QuestionId, List<UpdateOptionDto> Options)
    : IRequest<RequestResult<UpdateOptionResponse>>;
}
