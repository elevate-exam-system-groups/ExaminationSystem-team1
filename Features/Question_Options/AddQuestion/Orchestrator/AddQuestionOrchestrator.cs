using ExaminationSystem.Features.QuestionFeatures.AddQuestion.DTOs;

namespace ExaminationSystem.Features.QuestionFeatures.AddQuestion.Orchestrator
{
    public record AddQuestionOrchestratorCommand : IRequest<RequestResult<AddQuestionResponseDto>>
    {
        public Guid QuizId { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public List<OptionDto> Options { get; set; }
    }
}
