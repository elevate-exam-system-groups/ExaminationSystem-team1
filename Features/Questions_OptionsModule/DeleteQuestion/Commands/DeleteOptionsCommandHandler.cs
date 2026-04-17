namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Commands
{
    public class DeleteOptionsCommandHandler
        : IRequestHandler<DeleteOptionsCommand, RequestResult<DeleteResponse>>
    {

        private readonly IGeneralRepository<Option> _optionRepo;
        public DeleteOptionsCommandHandler(IGeneralRepository<Option> optionRepo)
          => _optionRepo = optionRepo;

        public async Task<RequestResult<DeleteResponse>> Handle(
            DeleteOptionsCommand request, CancellationToken ct)
        {

            var optionIds = await _optionRepo
                .Get(o => o.QuestionId == request.QuestionId && !o.isDeleted)
                .Select(o => o.Id)
                .ToListAsync(ct);

            if (optionIds.Any())
            {
                foreach (var id in optionIds)
                {
                    _optionRepo.UpdateInclude(
                        new Option
                        {
                            Id = id,
                            isDeleted = true,
                            DeletedAt = DateTime.UtcNow
                        },
                        nameof(Option.isDeleted),
                        nameof(Option.DeletedAt)
                    );
                }
            }

            return RequestResult<DeleteResponse>.Success(
                new DeleteResponse(true),
                $"{optionIds.Count} options deleted");
        }
    }

}