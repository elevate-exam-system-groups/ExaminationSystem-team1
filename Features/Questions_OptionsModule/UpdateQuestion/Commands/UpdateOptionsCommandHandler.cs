namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Commands
{
    public class UpdateOptionsCommandHandler
      : IRequestHandler<UpdateOptionsCommand, RequestResult<UpdateQuestionResponse>>
    {
        private readonly IGeneralRepository<Option> _optionRepo;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOptionsCommandHandler(
            IGeneralRepository<Option> optionRepo,
            IUnitOfWork unitOfWork)
        {
            _optionRepo = optionRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResult<UpdateQuestionResponse>> Handle(
            UpdateOptionsCommand request, CancellationToken ct)
        {
            // get existing options 
            var existingOptions = await _optionRepo
                .Get(o => o.QuestionId == request.QuestionId)
                .ToListAsync(ct);

            var incomingIds = request.Options
                .Where(o => o.Id.HasValue)
                .Select(o => o.Id!.Value)
                .ToHashSet();

            // Soft delete options not in the incoming list
            foreach (var opt in existingOptions.Where(o => !incomingIds.Contains(o.Id)))
                _optionRepo.SoftDelete(opt);

            // Update existing or add new options
            foreach (var optDto in request.Options)
            {
                if (optDto.Id.HasValue)
                {
                    var existing = existingOptions.FirstOrDefault(o => o.Id == optDto.Id.Value);
                    if (existing != null)
                    {
                        existing.Text = optDto.Text;
                        existing.IsCorrect = optDto.IsCorrect;
                        existing.UpdatedAt = DateTime.UtcNow;
                    }
                }
                else
                {
                    _optionRepo.Add(new Option
                    {
                        QuestionId = request.QuestionId,
                        Text = optDto.Text,
                        IsCorrect = optDto.IsCorrect
                    });
                }
            }


            return RequestResult<UpdateQuestionResponse>.Success(
                new UpdateQuestionResponse(request.QuestionId),
                "Options updated successfully");
        }
    }
}
