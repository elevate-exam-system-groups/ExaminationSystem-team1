using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.Orchestrator.Delete
{
    public record DeleteQuestionOrchestrator(Guid Id)
     : IRequest<RequestResult<DeleteQuestionResponse>>;
}
