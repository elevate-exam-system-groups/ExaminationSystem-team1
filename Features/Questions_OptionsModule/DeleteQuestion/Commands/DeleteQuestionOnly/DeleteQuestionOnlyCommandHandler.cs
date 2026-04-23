using ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Queries.GetQuestionForEdit;

namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Commands.DeleteQuestionOnly
{
    public class DeleteQuestionOnlyCommandHandler
     : IRequestHandler<DeleteQuestionOnlyCommand, RequestResult<DeleteResponse>>
    {

        private readonly IMediator _mediator;
        private readonly IGeneralRepository<Question> _questionRepo;
        public DeleteQuestionOnlyCommandHandler(
            IMediator mediator, IGeneralRepository<Question> questionRepo)
        {
            _mediator = mediator;
            _questionRepo = questionRepo;
        }

        public async Task<RequestResult<DeleteResponse>> Handle(
            DeleteQuestionOnlyCommand request, CancellationToken ct)
        {

            var validationResult = await _mediator.Send(
                new GetQuestionUpdateStatusQuery(request.QuestionId), ct);

            _questionRepo.UpdateInclude(
                new Question
                {
                    Id = request.QuestionId,
                    isDeleted = true,
                    DeletedAt = DateTime.UtcNow
                },
                nameof(Question.isDeleted),
                nameof(Question.DeletedAt)
            );

            _questionRepo.SaveChangesAsync();

            return RequestResult<DeleteResponse>.Success(
                new DeleteResponse(true),
                "Question deleted");
        }
    }
}
