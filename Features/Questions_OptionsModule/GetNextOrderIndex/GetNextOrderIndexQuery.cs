using ExaminationSystem.Features.Questions_OptionsModule.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.GetNextOrderIndex
{
    public record GetNextOrderIndexQuery(Guid QuizId) : IRequest<RequestResult<GetNextOrderIndexResponse>>;
    
}
