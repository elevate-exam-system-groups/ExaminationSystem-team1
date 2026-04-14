namespace ExaminationSystem.Features.Questions_OptionsModule.QuestionQuizStatus
{
    public class GetQuestionQuizStatusQueryHandler :
        IRequestHandler<GetQuestionQuizStatusQuery, RequestResult<QuestionStatusResult>>
    {

        private readonly IUnitOfWork _unitOfWork;
        public GetQuestionQuizStatusQueryHandler(IUnitOfWork unitOfWork)
          => _unitOfWork = unitOfWork;

        public async Task<RequestResult<QuestionStatusResult>> Handle(GetQuestionQuizStatusQuery request, CancellationToken ct)
        {
            var question = await _unitOfWork.GetRepository<Question>()
                .Get(q => q.Id == request.QuestionId)
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(ct);

            if (question == null) 
                return RequestResult<QuestionStatusResult>.Failure("Question not found");

            return RequestResult<QuestionStatusResult>.Success(
                new QuestionStatusResult(question.Quiz.Status, question.QuizId)
            );
        }
    }
}
