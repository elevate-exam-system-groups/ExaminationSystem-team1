namespace ExaminationSystem.Features.QuizModule.SubmitQuiz.Orchestrators
{
    public record SubmitQuizAttemptOrchestrator(string StudentId, Guid attemptId) : IRequest<RequestResult<bool>>;

    public class SubmitQuizAttemptOrchestratorValidator : AbstractValidator<SubmitQuizAttemptOrchestrator>
    {
        public SubmitQuizAttemptOrchestratorValidator()
        {

        }
    }
}
