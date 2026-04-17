using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.Common.Query
{ 
    public record GetQuestionInfoQuery(Guid QuestionId)
        : IRequest<RequestResult<QuestionInfoDto>>;

}
