using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.Common.Query
{
    public record CheckActiveAttemptsQuery(Guid QuizId)
     : IRequest<RequestResult<ActiveAttemptsDto>>;
}
