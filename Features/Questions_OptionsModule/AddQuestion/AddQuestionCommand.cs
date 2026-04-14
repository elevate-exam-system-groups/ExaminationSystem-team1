using ExaminationSystem.Features.Questions_OptionsModule.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion
{
    public record AddQuestionCommand : IRequest<RequestResult<AddQuestionResponse>>
    {
        public Guid QuizId { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public List<OptionDto> Options { get; set; }
    }

}