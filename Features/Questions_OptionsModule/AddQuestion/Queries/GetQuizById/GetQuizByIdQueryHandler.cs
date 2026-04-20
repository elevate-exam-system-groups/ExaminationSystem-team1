namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Queries.GetQuiz
{
    public class GetQuizByIdQueryHandler
    : IRequestHandler<GetQuizByIdQuery, RequestResult<QuizDto>>
    {

        private readonly IGeneralRepository<Quiz> _quizRepo;
        public GetQuizByIdQueryHandler(IGeneralRepository<Quiz> quizRepo)
            => _quizRepo = quizRepo;

        public async Task<RequestResult<QuizDto>> Handle(
             GetQuizByIdQuery request, CancellationToken ct)
        {
            var quiz = await _quizRepo
                .GetById(request.quizId)
                .Select(q => new QuizDto
                (
                    q.Id,
                    q.Status
                )).FirstOrDefaultAsync(ct);

            if (quiz == null)
                return RequestResult<QuizDto>.Failure(
                    "Quiz not found",
                    RequestErrorCode.NotFound);

            return RequestResult<QuizDto>.Success(quiz);
        }

    }
}
