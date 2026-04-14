using ExaminationSystem.Features.Questions_OptionsModule.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion
{
    public record DeleteQuestionCommand : IRequest<RequestResult<DeleteQuestionResponse>>
    {
        public Guid Id { get; set; }
    }

}
