using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.Query.QuestionForEdit
{
    public record GetQuestionForEditQuery(Guid QuestionId)
    : IRequest<RequestResult<QuestionForEditDto>>;
}
