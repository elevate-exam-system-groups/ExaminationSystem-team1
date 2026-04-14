namespace ExaminationSystem.Features.Questions_OptionsModule.Orchestrator.UpdateQuestion
{
    public class UpdateQuestionOrchestratorHandler
        : IRequestHandler<UpdateQuestionOrchestrator, RequestResult<UpdateQuestionResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        public UpdateQuestionOrchestratorHandler(IUnitOfWork unitOfWork)
         => _unitOfWork = unitOfWork;


        public async Task<RequestResult<UpdateQuestionResponse>> Handle
                  (UpdateQuestionOrchestrator request, CancellationToken cancellationToken)
        {

            var questionRepo = _unitOfWork.GetRepository<Question>();
            var question = await questionRepo.GetByIdWithTracking(request.Id)
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(cancellationToken);

            if (question == null)
                return RequestResult<UpdateQuestionResponse>.Failure("Question not found");

            // Update question fields
            question.Text = request.Text;
            question.Explanation = request.Explanation;
            question.UpdatedAt = DateTime.Now;  // Use DateTime.Now to match BaseModel

            // Handle Options with tracking
            var optionRepo = _unitOfWork.GetRepository<Option>();

            var existingOptions = await optionRepo.Get(o => o.QuestionId == question.Id)
                .AsTracking()
                .ToListAsync(cancellationToken);

            var incomingOptionIds = request.Options
                .Where(o => o.Id.HasValue)
                .Select(o => o.Id.Value)
                .ToList();

            // Soft delete options not in incoming list
            foreach (var opt in existingOptions.Where(o => !incomingOptionIds.Contains(o.Id)))
            {
                optionRepo.SoftDelete(opt);
            }

            // Update or Add options
            foreach (var opt in request.Options)
            {
                if (opt.Id.HasValue)
                {
                    var existingOpt = existingOptions.FirstOrDefault(o => o.Id == opt.Id.Value);
                    if (existingOpt != null)
                    {
                        existingOpt.Text = opt.Text;
                        existingOpt.IsCorrect = opt.IsCorrect;
                        existingOpt.UpdatedAt = DateTime.Now;
                    }
                }
                else
                {
                    optionRepo.Add(new Option
                    {
                        QuestionId = question.Id,
                        Text = opt.Text,
                        IsCorrect = opt.IsCorrect
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return RequestResult<UpdateQuestionResponse>.Success(
                new UpdateQuestionResponse(question.Id),
                "Question updated successfully.");
        }
    }
}