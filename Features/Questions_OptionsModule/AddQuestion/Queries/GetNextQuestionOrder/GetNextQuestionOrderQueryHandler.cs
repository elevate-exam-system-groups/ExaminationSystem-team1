namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Queries
{

    public class GetNextQuestionOrderQueryHandler
        : IRequestHandler<GetNextQuestionOrderQuery, RequestResult<NextQuestionOrderDto>>
    {

        private readonly IGeneralRepository<Question> _questionRepo;
        public GetNextQuestionOrderQueryHandler(IGeneralRepository<Question> questionRepo)
            => _questionRepo = questionRepo;

        public async Task<RequestResult<NextQuestionOrderDto>> Handle(
            GetNextQuestionOrderQuery request,
            CancellationToken ct)
        {
            var maxOrder = await _questionRepo
                .Get(q => q.QuizId == request.QuizId && !q.isDeleted)
                .MaxAsync(q => (int?)q.OrderIndex, ct) ?? 0;

            return RequestResult<NextQuestionOrderDto>.Success(
                new NextQuestionOrderDto(maxOrder + 1));
        }
    }
}
