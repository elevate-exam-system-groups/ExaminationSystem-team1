using ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Commands.DeleteOptions
{
    public class DeleteOptionsCommandHandler
        : IRequestHandler<DeleteOptionsCommand, RequestResult<DeleteResponseDto>>
    {

        private readonly IGeneralRepository<QuestionOption> _optionRepo;
        public DeleteOptionsCommandHandler(IGeneralRepository<QuestionOption> optionRepo)
            => _optionRepo = optionRepo;

        public async Task<RequestResult<DeleteResponseDto>> Handle(
            DeleteOptionsCommand request, CancellationToken ct)
        {

            var deletedCount = await _optionRepo
                .Get(o => o.QuestionId == request.QuestionId && !o.IsDeleted)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(o => o.IsDeleted, true)
                    .SetProperty(o => o.DeletedAt, DateTime.UtcNow),
                ct);

            return RequestResult<DeleteResponseDto>.Success(
                new DeleteResponseDto(true),
                       $"options deleted");
        }
    }
}