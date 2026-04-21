

namespace ExaminationSystem.Features.AnswerQuestion.Requests
{
    public record GetQuizIdForCurrentQuestionQueryRequest(Guid QuestionId)
        : IRequest<RequestResult<Guid>>;

    public class GetQuizIdForCurrentQuestionQueryRequestHandler
        : IRequestHandler<GetQuizIdForCurrentQuestionQueryRequest, RequestResult<Guid>>
    {
        private readonly IGeneralRepository<Question> _questionRepository;

        public GetQuizIdForCurrentQuestionQueryRequestHandler(IGeneralRepository<Question> questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<RequestResult<Guid>> Handle(GetQuizIdForCurrentQuestionQueryRequest request, CancellationToken cancellationToken)
        {
            Guid quizId = await _questionRepository
                .Get(qu => qu.Id == request.QuestionId)
                .Select(qu => qu.QuizId).FirstOrDefaultAsync();

            return RequestResult<Guid>.Success(quizId);
        }
    }
}
