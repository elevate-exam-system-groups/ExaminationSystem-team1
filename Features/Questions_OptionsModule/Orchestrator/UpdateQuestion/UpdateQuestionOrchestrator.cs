namespace ExaminationSystem.Features.Questions_OptionsModule.Orchestrator.UpdateQuestion
{
    public record UpdateQuestionOrchestrator : IRequest<RequestResult<UpdateQuestionResponse>>
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public List<UpdateOptionDto> Options { get; set; }
    }

}
