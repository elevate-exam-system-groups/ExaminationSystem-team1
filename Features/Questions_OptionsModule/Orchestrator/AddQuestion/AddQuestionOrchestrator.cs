namespace ExaminationSystem.Features.Questions_OptionsModule.Orchestrator.AddQuestion
{
    public record AddQuestionOrchestrator : IRequest<RequestResult<AddQuestionResponse>>
    {
        public Guid QuizId { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public List<OptionDto> Options { get; set; }
    }

}