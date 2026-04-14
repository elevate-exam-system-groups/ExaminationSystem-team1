using ExaminationSystem.Features.Questions_OptionsModule.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.CreateQuestion
{
    public record CreateQuestionCommand : IRequest<RequestResult<CreateQuestionResponse>>
    {
        public Guid QuizId { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public int OrderIndex { get; set; }
    }
}
