using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.QuestionFeatures.AddQuestion.DTOs;

namespace ExaminationSystem.Features.QuestionFeatures.AddQuestion.Commands.CreateOptionsForQuestion
{
    public record CreateOptionsForQuestionCommand(Guid QuestionId, List<OptionDto> Options)
        : IRequest<RequestResult<CreateOptionsResponseDto>>;
}
