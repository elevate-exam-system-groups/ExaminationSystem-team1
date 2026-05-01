using ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.QuestionFeatures.DeleteQuestion.Commands.DeleteQuestionOnly
{
    public class DeleteQuestionOnlyCommandHandler
     : IRequestHandler<DeleteQuestionOnlyCommand, RequestResult<DeleteResponseDto>>
    {

        private readonly IGeneralRepository<Question> _questionRepo;
        public DeleteQuestionOnlyCommandHandler(
            IMediator mediator, IGeneralRepository<Question> questionRepo)
        {
            _questionRepo = questionRepo;
        }

        public async Task<RequestResult<DeleteResponseDto>> Handle(
            DeleteQuestionOnlyCommand request, CancellationToken ct)
        {

            var question = await _questionRepo
                .GetByIdWithTracking(request.QuestionId)
                .FirstOrDefaultAsync(ct);

            if (question is null)
                return RequestResult<DeleteResponseDto>.Failure(
                    "Question not found", RequestErrorCode.NotFound);

            _questionRepo.SoftDelete(question);
            await _questionRepo.SaveChangesAsync();

            return RequestResult<DeleteResponseDto>.Success(
                new DeleteResponseDto(true),
                "Question deleted");
        }
    }
}
