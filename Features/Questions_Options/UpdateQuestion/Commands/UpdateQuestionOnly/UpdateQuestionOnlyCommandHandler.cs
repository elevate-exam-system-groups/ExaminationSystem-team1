namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Commands.UpdateQuestionOnly
{
    public class UpdateQuestionOnlyCommandHandler
      : IRequestHandler<UpdateQuestionOnlyCommand, RequestResult<UpdateQuestionOnlyResponseDto>>
    {

        private readonly IGeneralRepository<Question> _questionRepo;
        public UpdateQuestionOnlyCommandHandler(
            IGeneralRepository<Question> questionRepo)
        {
            _questionRepo = questionRepo;
        }

        public async Task<RequestResult<UpdateQuestionOnlyResponseDto>> Handle(
            UpdateQuestionOnlyCommand request, CancellationToken ct)
        {

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

            return RequestResult<UpdateQuestionOnlyResponseDto>.Success(
                new UpdateQuestionOnlyResponseDto(true),
                "Question updated successfully");
        }
    }

}

