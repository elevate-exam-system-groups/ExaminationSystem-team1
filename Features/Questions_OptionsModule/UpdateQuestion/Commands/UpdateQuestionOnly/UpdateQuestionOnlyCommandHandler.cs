
using ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Queries.GetQuestionForEdit;

namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Commands.UpdateQuestionOnly
{
    public class UpdateQuestionOnlyCommandHandler
      : IRequestHandler<UpdateQuestionOnlyCommand, RequestResult<UpdateQuestionResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IGeneralRepository<Question> _questionRepo;

        public UpdateQuestionOnlyCommandHandler(
            IMediator mediator,
            IGeneralRepository<Question> questionRepo)
        {
            _mediator = mediator;
            _questionRepo = questionRepo;
        }

        public async Task<RequestResult<UpdateQuestionResponse>> Handle(
            UpdateQuestionOnlyCommand request, CancellationToken ct)
        {


            var questionDTO = await _mediator.Send(
                new GetQuestionByIdQuery(request.QuestionId), ct);

            if (!questionDTO.IsSuccess)
                return RequestResult<UpdateQuestionResponse>.Failure(
                    questionDTO.Message, questionDTO.requestErrorCode);


            var Result = questionDTO.Data;

            if (Result.QuizStatus == QuizStatus.Published)
                return RequestResult<UpdateQuestionResponse>.Failure(
                    "Cannot update question in a published quiz. Unpublish quiz first.",
                    RequestErrorCode.Conflict);

            _questionRepo.UpdateInclude(new Question
            {
                Id = request.QuestionId,
                Text = request.Text,
                Explanation = request.Explanation,
                UpdatedAt = DateTime.UtcNow
            },
               nameof(Question.Text),
               nameof(Question.Explanation),
               nameof(Question.UpdatedAt)
           );

            await _questionRepo.SaveChangesAsync();

            return RequestResult<UpdateQuestionResponse>.Success(
                new UpdateQuestionResponse(request.QuestionId),
                "Question updated successfully");
        }
    }

}

