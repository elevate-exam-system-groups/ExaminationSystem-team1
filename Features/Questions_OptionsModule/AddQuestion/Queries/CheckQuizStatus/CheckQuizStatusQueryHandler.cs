namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Queries.GetQuiz
{
    public class CheckQuizStatusQueryHandler
    : IRequestHandler<CheckQuizStatusQuery, RequestResult<QuizStatusDto>>
    {

        private readonly IGeneralRepository<Quiz> _quizRepo;
        public CheckQuizStatusQueryHandler(IGeneralRepository<Quiz> quizRepo)
            => _quizRepo = quizRepo;

        public async Task<RequestResult<QuizStatusDto>> Handle(
             CheckQuizStatusQuery request, CancellationToken ct)
        {
            var quizStatus = await _quizRepo
                .GetById(request.quizId)
                .Select(q => new QuizStatusDto( q.Status)).FirstOrDefaultAsync(ct);

            if (quizStatus == null)
                return RequestResult<QuizStatusDto>.Failure(
                    "Quiz not found",
                    RequestErrorCode.NotFound);


            if (quizStatus.Status == QuizStatus.Published)
                return RequestResult<QuizStatusDto>.Failure(
                    "Cannot add question to published quiz",
                    RequestErrorCode.Conflict);

            return RequestResult<QuizStatusDto>.Success(quizStatus);
        }

    }
}
