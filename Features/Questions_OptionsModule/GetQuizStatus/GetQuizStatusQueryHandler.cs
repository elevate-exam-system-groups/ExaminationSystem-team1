namespace ExaminationSystem.Features.Questions_OptionsModule.GetQuizStatus
{

    public class GetQuizStatusQueryHandler : IRequestHandler<GetQuizStatusQuery, RequestResult<QuizStatus>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetQuizStatusQueryHandler(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;

        public async Task<RequestResult<QuizStatus>> Handle(GetQuizStatusQuery request, CancellationToken ct)
        {
            var quiz = await _unitOfWork.GetRepository<Quiz>()
                .GetById(request.QuizId)
                .FirstOrDefaultAsync(ct);

            if (quiz == null)
                return RequestResult<QuizStatus>.Failure("Quiz not found");

            return RequestResult<QuizStatus>.Success(quiz.Status);
        }
    }
}
