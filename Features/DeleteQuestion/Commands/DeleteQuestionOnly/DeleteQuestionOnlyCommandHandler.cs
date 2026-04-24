using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.DeleteQuestion.Dtos;

namespace ExaminationSystem.Features.DeleteQuestion.Commands.DeleteQuestionOnly
{
    public class DeleteQuestionOnlyCommandHandler
     : IRequestHandler<DeleteQuestionOnlyCommand, RequestResult<DeleteResponseDto>>
    {

        private readonly IMediator _mediator;
        private readonly IGeneralRepository<Question> _questionRepo;
        public DeleteQuestionOnlyCommandHandler(
            IMediator mediator, IGeneralRepository<Question> questionRepo)
        {
            _mediator = mediator;
            _questionRepo = questionRepo;
        }

        public async Task<RequestResult<DeleteResponseDto>> Handle(
            DeleteQuestionOnlyCommand request, CancellationToken ct)
        {

            var questionToDelete = new Question { Id = request.QuestionId };
            _questionRepo.SoftDelete(questionToDelete);

            await _questionRepo.SaveChangesAsync();

            return RequestResult<DeleteResponseDto>.Success(
                new DeleteResponseDto(true),
                "Question deleted");
        }
    }
}
