using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.Orchestrator.Update
{
    public record UpdateQuestionOptionsOrchestrator : IRequest<RequestResult<UpdateQuestionResponse>>
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public List<UpdateOptionDto> Options { get; set; }
    }

}
