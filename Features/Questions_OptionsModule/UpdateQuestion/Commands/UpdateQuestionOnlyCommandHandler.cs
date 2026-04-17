namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Commands
{
    public class UpdateQuestionOnlyCommandHandler
      : IRequestHandler<UpdateQuestionOnlyCommand, RequestResult<UpdateQuestionResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IGeneralRepository<Question> _questionRepo;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateQuestionOnlyCommandHandler(
            IMediator mediator,
            IGeneralRepository<Question> questionRepo,
            IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _questionRepo = questionRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResult<UpdateQuestionResponse>> Handle(
            UpdateQuestionOnlyCommand request, CancellationToken ct)
        {

            var questionDTO = await _mediator.Send(
                new GetQuestionForEditQuery(request.QuestionId), ct);

            if (!questionDTO.IsSuccess)
                return RequestResult<UpdateQuestionResponse>.Failure(
                    questionDTO.Message, questionDTO.requestErrorCode);

            var Result = questionDTO.Data;

            _questionRepo.UpdateInclude(new Question
            {
                Id = Result.QuestionId,
                Text = request.Text,
                Explanation = request.Explanation,
                UpdatedAt = DateTime.UtcNow
            },
               nameof(Question.Text),
               nameof(Question.Explanation),
               nameof(Question.UpdatedAt)
           );


            return RequestResult<UpdateQuestionResponse>.Success(
                new UpdateQuestionResponse(Result.QuestionId),
                "Question updated successfully");
        }
    }

}

