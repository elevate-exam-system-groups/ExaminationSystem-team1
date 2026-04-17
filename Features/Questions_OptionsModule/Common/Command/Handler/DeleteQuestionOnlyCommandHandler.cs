using ExaminationSystem.Features.Questions_OptionsModule.Common.Command.Request;
using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;
using ExaminationSystem.Features.Questions_OptionsModule.Query.QuestionForEdit;

namespace ExaminationSystem.Features.Questions_OptionsModule.Common.Command.Handler
{
    public class DeleteQuestionOnlyCommandHandler
     : IRequestHandler<DeleteQuestionOnlyCommand, RequestResult<DeleteQuestionOnlyResponse>>
    {

        private readonly IMediator _mediator;
        private readonly IGeneralRepository<Question> _questionRepo;
        public DeleteQuestionOnlyCommandHandler(
            IMediator mediator, IGeneralRepository<Question> questionRepo)
        {
            _mediator = mediator;
            _questionRepo = questionRepo;
        }

        public async Task<RequestResult<DeleteQuestionOnlyResponse>> Handle(
            DeleteQuestionOnlyCommand request, CancellationToken ct)
        {
            var validationResult = await _mediator.Send(
                new GetQuestionForEditQuery(request.QuestionId), ct);

            if (!validationResult.IsSuccess)
                return RequestResult<DeleteQuestionOnlyResponse>.Failure(
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

            return RequestResult<DeleteQuestionOnlyResponse>.Success(
                new DeleteQuestionOnlyResponse(true),
                "Question deleted");
        }
    }
}
