namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion
{
    public record DeleteQuestionOrchestrator(Guid Id)
     : IRequest<RequestResult<DeleteResponse>>;
}
