using ExaminationSystem.Features.Questions_OptionsModule.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion
{
    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, RequestResult<UpdateQuestionResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateQuestionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResult<UpdateQuestionResponse>> Handle
            (UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            // Use tracking to be able to update
            var questionRepo = _unitOfWork.GetRepository<Question>();
            var question = await questionRepo.GetByIdWithTracking(request.Id)
                                             .Include(q => q.Quiz)
                                             .FirstOrDefaultAsync(cancellationToken);


            if (question == null)
                return RequestResult<UpdateQuestionResponse>.Failure("Question not found");

            // Optional: Check if quiz is published - maybe restrict editing?
            // Business rule: Can update questions even if quiz is published (but can't delete)
            // We'll allow updates always
            question.Text = request.Text;
            question.Explanation = request.Explanation;
            question.UpdatedAt = DateTime.UtcNow;

            // Handle Options with tracking
            var optionRepo = _unitOfWork.GetRepository<Option>();

            // Get All options => This Question
            var existingOptions = await optionRepo.Get(o => o.QuestionId == question.Id)
                                                  .AsTracking()
                                                  .ToListAsync(cancellationToken);

            // Get incoming option IDs(Question) (those that have Ids, meaning they are updates, not new)
            var incomingOptionIds = request.Options.Where(o => o.Id.HasValue)
                                                   .Select(o => o.Id.Value)
                                                   .ToList();

            // Soft delete options not in incoming list
            foreach (var opt in existingOptions.Where(o => !incomingOptionIds.Contains(o.Id)))
            {
                optionRepo.SoftDelete(opt);
            }

            // Update or Add
            foreach (var opt in request.Options)
            {
                if (opt.Id.HasValue)
                {
                    var existingOpt = existingOptions.FirstOrDefault(o => o.Id == opt.Id.Value);
                    if (existingOpt != null)
                    {
                        existingOpt.Text = opt.Text;
                        existingOpt.IsCorrect = opt.IsCorrect;
                        existingOpt.UpdatedAt = DateTime.UtcNow;
                        // No need to call Update() if entity is tracked
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