using ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Dtos;

namespace ExaminationSystem.Features.QuestionFeatures.UpdateQuestion.Queries.QuestionUpdateStatus
{
    public class GetQuestionUpdateStatusQueryHandler
     : IRequestHandler<GetQuestionUpdateStatusQuery, RequestResult<QuestionUpdateStatusDto>>
    {

        private readonly IGeneralRepository<Question> _questionRepo;
        public GetQuestionUpdateStatusQueryHandler(IGeneralRepository<Question> questionRepo)
            => _questionRepo = questionRepo;

        public async Task<RequestResult<QuestionUpdateStatusDto>> Handle(
            GetQuestionUpdateStatusQuery request, CancellationToken ct)
        {
            var Status = await _questionRepo
                .Get(q => q.Id == request.QuestionId)
                .Select(q => new QuestionUpdateStatusDto(q.Quiz!.Status))  //====
                .FirstOrDefaultAsync(ct);

            if (Status == null)
                return RequestResult<QuestionUpdateStatusDto>.Failure(
                    "Question not found", RequestErrorCode.NotFound);


            return RequestResult<QuestionUpdateStatusDto>.Success(Status);
        }
    }
}

