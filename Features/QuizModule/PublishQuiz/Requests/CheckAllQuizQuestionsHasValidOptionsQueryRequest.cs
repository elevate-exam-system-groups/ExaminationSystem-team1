namespace ExaminationSystem.Features.QuizModule.PublishQuiz.Requests
{
    public record CheckAllQuizQuestionsHasValidOptionsQueryRequest(Guid quizId) : IRequest<RequestResult<bool>>;

    public class CheckAllQuizQuestionsHasValidOptionsQueryRequestHandler
        : IRequestHandler<CheckAllQuizQuestionsHasValidOptionsQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Question> _questionRepository;

        public CheckAllQuizQuestionsHasValidOptionsQueryRequestHandler(IGeneralRepository<Question> questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<RequestResult<bool>> Handle(CheckAllQuizQuestionsHasValidOptionsQueryRequest request, CancellationToken cancellationToken)
        {
            var HasInValidQuestions = await _questionRepository
                 .Get(q => q.QuizId == request.quizId)
                 .AnyAsync(q => q.Options == null ||
                                  q.Options.Count < 2 ||
                                  !q.Options.Any(o => o.IsCorrect));

            if (HasInValidQuestions)
            {
                return RequestResult<bool>
                    .Failure("All questions must have at least 2 options and at least one correct option.", RequestErrorCode.Forbidden);
            }

            return RequestResult<bool>.Success(true);
        }
    }


}
