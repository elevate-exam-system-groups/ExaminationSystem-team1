using ExaminationSystem.Features.Questions_OptionsModule.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.Orchestrator.DeleteQuestion
{
    public record DeleteQuestionOrchestrator(Guid Id) : IRequest<RequestResult<DeleteQuestionResponse>>;

}
