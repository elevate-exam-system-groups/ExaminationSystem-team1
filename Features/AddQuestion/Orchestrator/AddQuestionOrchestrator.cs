using ExaminationSystem.Features.AddQuestion.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.AddQuestion.Orchestrator
{
    public record AddQuestionOrchestratorCommand : IRequest<RequestResult<AddQuestionResponseDto>>
    {
        public Guid QuizId { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public List<OptionDto> Options { get; set; }
    }
}
