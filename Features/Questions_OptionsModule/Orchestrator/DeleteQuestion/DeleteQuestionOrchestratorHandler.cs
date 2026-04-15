    namespace ExaminationSystem.Features.Questions_OptionsModule.Orchestrator.DeleteQuestion
    {
    public class DeleteQuestionOrchestratorHandler 
        : IRequestHandler<DeleteQuestionOrchestrator, RequestResult<DeleteQuestionResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        public DeleteQuestionOrchestratorHandler(IUnitOfWork unitOfWork)
          => _unitOfWork = unitOfWork;
        

        public async Task<RequestResult<DeleteQuestionResponse>> 
            Handle(DeleteQuestionOrchestrator request, CancellationToken cancellationToken)
        {

            var questionRepo = _unitOfWork.GetRepository<Question>();

            // IMPORTANT: Include Quiz to check its status
            var question = await questionRepo.GetById(request.Id)
                                             .Include(q => q.Quiz)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (question == null)
                return RequestResult<DeleteQuestionResponse>.Failure("Question not found");

            // BUSINESS RULE: Cannot delete from published quiz
            if (question.Quiz.Status == QuizStatus.Published)
            {
                return RequestResult<DeleteQuestionResponse>.Failure(
                    "Cannot delete question from a published quiz. Please unpublish the quiz first.",
                    RequestErrorCode.Conflict);
            }

            // Soft delete question
            questionRepo.SoftDelete(question);

            // Soft delete all options
            var optionRepo = _unitOfWork.GetRepository<Option>();
            var options = await optionRepo.Get(o => o.QuestionId == question.Id)
                                          .ToListAsync(cancellationToken);
            foreach (var opt in options)
            {
                optionRepo.SoftDelete(opt);
            }

            await _unitOfWork.SaveChangesAsync();

            return RequestResult<DeleteQuestionResponse>.Success(
                new DeleteQuestionResponse(true),
                "Question soft-deleted successfully.");
        }
    }

}