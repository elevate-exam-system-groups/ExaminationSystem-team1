namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Orchestrators
{
    public record DeleteQuestionOrchestrator(Guid Id)
     : IRequest<RequestResult<DeleteResponseDto>>;
}
