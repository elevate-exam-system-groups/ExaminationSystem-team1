using ExaminationSystem.Features.AddQuestion.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.AddQuestion.Commands.CreateOptionsForQuestion
{
    public record CreateOptionsForQuestionCommand(Guid QuestionId, List<OptionDto> Options)
        : IRequest<RequestResult<CreateOptionsResponseDto>>;
}
