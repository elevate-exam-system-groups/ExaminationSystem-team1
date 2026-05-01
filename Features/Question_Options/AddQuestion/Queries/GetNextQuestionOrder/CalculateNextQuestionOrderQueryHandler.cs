using ExaminationSystem.Features.QuestionFeatures.AddQuestion.DTOs;

namespace ExaminationSystem.Features.QuestionFeatures.AddQuestion.Queries.GetNextQuestionOrder
{

    public class CalculateNextQuestionOrderQueryHandler
        : IRequestHandler<CalculateNextQuestionOrderQuery, RequestResult<NextQuestionOrderDto>>
    {

        private readonly IGeneralRepository<Question> _questionRepo;
        public CalculateNextQuestionOrderQueryHandler(IGeneralRepository<Question> questionRepo)
        {
            _questionRepo = questionRepo;
        }

        public async Task<RequestResult<NextQuestionOrderDto>> Handle(
            CalculateNextQuestionOrderQuery request, CancellationToken ct)
        {

            var maxOrder = await _questionRepo
                .Get(q => q.QuizId == request.QuizId)
                .MaxAsync(q => (int?)q.OrderIndex, ct) ?? 0;

            return RequestResult<NextQuestionOrderDto>.Success(
                new NextQuestionOrderDto(maxOrder + 1));
        }
    }
}
