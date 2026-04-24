using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.DeleteQuestion.Commands.DeleteOptions
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

            var optionsToDelete = await _optionRepo
                .Get(o => o.QuestionId == request.QuestionId && !o.isDeleted)
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

            return RequestResult<DeleteResponseDto>.Success(
                new DeleteResponseDto(true),
                $"options deleted");  
        }
    }

}