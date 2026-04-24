namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.Queries.GetQuestionInfo
{
    public class GetQuestionInfoQueryHandler
          : IRequestHandler<GetQuestionInfoQuery, RequestResult<QuestionInfoDto>>
    {

        private readonly IGeneralRepository<Question> _questionRepo;
        public GetQuestionInfoQueryHandler(IGeneralRepository<Question> questionRepo)
           => _questionRepo = questionRepo;

        public async Task<RequestResult<QuestionInfoDto>> Handle(
            GetQuestionInfoQuery request, CancellationToken ct)
        {
            var questionInfo = await _questionRepo
                .Get(q => q.Id == request.QuestionId && !q.isDeleted)
                .Select(q => new QuestionInfoDto(
                    //q.Id,
                    q.QuizId,
                    q.Quiz.Status
                )).FirstOrDefaultAsync(ct);

            if (questionInfo == null)
            {
                return RequestResult<QuestionInfoDto>.Failure(
                    "Question not found",
                    RequestErrorCode.NotFound);
            }

            return RequestResult<QuestionInfoDto>.Success(questionInfo);
        }
    }
}