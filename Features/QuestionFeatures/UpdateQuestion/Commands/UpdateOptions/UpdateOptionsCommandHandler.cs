using ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Dtos;

namespace ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Commands.UpdateOptions
{
    public class UpdateOptionsCommandHandler
      : IRequestHandler<UpdateOptionsCommand, RequestResult<UpdateOptionResponse>>
    {

        private readonly IGeneralRepository<QuestionOption> _optionRepo;
        public UpdateOptionsCommandHandler(IGeneralRepository<QuestionOption> optionRepo)
           => _optionRepo = optionRepo;

        public async Task<RequestResult<UpdateOptionResponse>> Handle(
            UpdateOptionsCommand request, CancellationToken ct)
        {

            var existingOptions = await _optionRepo
                .Get(o => o.QuestionId == request.QuestionId)
                .ToListAsync(ct);

            var incomingIds = request.Options
                .Where(o => o.Id.HasValue)
                .Select(o => o.Id!.Value)
                .ToList();
            //.ToHashSet();

            foreach (var opt in existingOptions.Where(o => !incomingIds.Contains(o.Id)))
                _optionRepo.SoftDelete(opt);

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
                    _optionRepo.Add(new QuestionOption
                    {
                        QuestionId = request.QuestionId,
                        Text = optDto.Text,
                        IsCorrect = optDto.IsCorrect
                    });
                }
            }

            await _optionRepo.SaveChangesAsync();

            return RequestResult<UpdateOptionResponse>.Success(
                new UpdateOptionResponse(true),
                "Options updated successfully");
        }
    }
}
