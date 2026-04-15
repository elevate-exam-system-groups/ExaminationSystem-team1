namespace ExaminationSystem.Features.Questions_OptionsModule.Orchestrator.UpdateQuestion
{

    #region Request
    public record UpdateQuestionOrchestrator : IRequest<RequestResult<UpdateQuestionResponse>>
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public List<UpdateOptionDto> Options { get; set; }
    }

    #endregion

    #region Validator

    public class UpdateQuestionValidator : AbstractValidator<UpdateQuestionViewModel>
    {
        public UpdateQuestionValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Text).NotEmpty()
                               .MaximumLength(1000);

            RuleFor(x => x.Options)
                .Must(x => x != null && x.Count >= 2)
                .WithMessage("At least 2 options are required.");

            RuleFor(x => x.Options)
                .Must(x => x != null && x.Count(o => o.IsCorrect) == 1)
                .WithMessage("Exactly one correct option required.");

            RuleForEach(x => x.Options).ChildRules(option =>
            {
                option.RuleFor(o => o.Text)
                        .NotEmpty()
                        .WithMessage("Option text cannot be empty");
            });
        }
    }

    #endregion

    #region Handler

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

    #endregion

}