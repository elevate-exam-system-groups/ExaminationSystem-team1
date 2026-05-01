using ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Dtos;

namespace ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Orchestrators
{
    public record UpdateQuestionOptionsOrchestrator : IRequest<RequestResult<UpdateQuestionResponseDto>>
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public List<UpdateOptionDto> Options { get; set; }
    }

}
