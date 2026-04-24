using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.DeleteQuestion.Orchestrators
{
    public record DeleteQuestionOrchestrator(Guid Id)
     : IRequest<RequestResult<DeleteResponseDto>>;
}
