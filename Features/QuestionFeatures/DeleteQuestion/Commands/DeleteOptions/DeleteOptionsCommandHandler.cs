using ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Commands.DeleteOptions
{
    public class DeleteOptionsCommandHandler
        : IRequestHandler<DeleteOptionsCommand, RequestResult<DeleteResponseDto>>
    {

        private readonly IGeneralRepository<Option> _optionRepo;
        public DeleteOptionsCommandHandler(IGeneralRepository<Option> optionRepo)
            => _optionRepo = optionRepo;

        public async Task<RequestResult<DeleteResponseDto>> Handle(
            DeleteOptionsCommand request, CancellationToken ct)
        {

<<<<<<< HEAD
            var deletedCount = await _optionRepo
                .Get(o => o.QuestionId == request.QuestionId && !o.IsDeleted)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(o => o.IsDeleted, true)
                    .SetProperty(o => o.DeletedAt, DateTime.UtcNow),
                ct);
=======
            var optionsToDelete = await _optionRepo
                .Get(o => o.QuestionId == request.QuestionId && !o.IsDeleted)
                .ToListAsync(ct);

            if (!optionsToDelete.Any())
            {
                return RequestResult<DeleteResponseDto>.Success(
                    new DeleteResponseDto(true),
                    "No options found to delete");
            }

            // alternative => ExecuteUpdate
            foreach (var option in optionsToDelete)
            {
                _optionRepo.SoftDelete(option);
            }


            await _optionRepo.SaveChangesAsync();
>>>>>>> QuizModule

            return RequestResult<DeleteResponseDto>.Success(
                new DeleteResponseDto(true),
                       $"options deleted");
        }
    }
}