namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Commands
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
                new GetQuestionForEditQuery(request.QuestionId), ct);

            if (!validationResult.IsSuccess)
                return RequestResult<DeleteResponse>.Failure(
                    validationResult.Message,
                    validationResult.requestErrorCode);

            // Soft Delete For Question Only
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

            return RequestResult<DeleteResponse>.Success(
                new DeleteResponse(true),
                "Question deleted");
        }
    }
}
