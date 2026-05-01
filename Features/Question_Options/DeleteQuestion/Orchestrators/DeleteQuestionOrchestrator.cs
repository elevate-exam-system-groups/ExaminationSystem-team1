using ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Orchestrators
{
    public record DeleteQuestionOrchestrator(Guid Id)
     : IRequest<RequestResult<DeleteResponseDto>>;
}
