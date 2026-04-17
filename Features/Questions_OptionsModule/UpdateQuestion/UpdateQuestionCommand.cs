using ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion
{
    public record UpdateQuestionCommand : IRequest<RequestResult<UpdateQuestionResponse>>
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public List<UpdateOptionDto> Options { get; set; }
    }
}
