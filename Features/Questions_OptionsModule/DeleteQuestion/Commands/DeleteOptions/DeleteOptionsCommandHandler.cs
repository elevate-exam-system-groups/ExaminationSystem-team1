namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Commands.DeleteOptions
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

            var optionsToDelete = await _optionRepo
                .Get(o => o.QuestionId == request.QuestionId && !o.isDeleted)
                .ToListAsync(ct);

            if (!optionsToDelete.Any())
            {
                return RequestResult<DeleteResponse>.Success(
                    new DeleteResponse(true),
                    "No options found to delete");
            }

            // alternative => ExecuteUpdate
            foreach (var option in optionsToDelete)
            {
                _optionRepo.SoftDelete(option);
            }


            await _optionRepo.SaveChangesAsync();

            return RequestResult<DeleteResponse>.Success(
                new DeleteResponse(true),
                $"options deleted");  
        }
    }

}