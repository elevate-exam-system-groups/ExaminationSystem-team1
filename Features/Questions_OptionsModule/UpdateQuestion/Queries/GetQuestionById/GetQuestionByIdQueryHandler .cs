namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Queries.GetQuestionForEdit
{
    public class GetQuestionForEditQueryHandler
     : IRequestHandler<GetQuestionByIdQuery, RequestResult<QuestionForEditDto>>
    {

        private readonly IGeneralRepository<Question> _questionRepo;
        public GetQuestionForEditQueryHandler(IGeneralRepository<Question> questionRepo)
            => _questionRepo = questionRepo;

        public async Task<RequestResult<QuestionForEditDto>> Handle(
            GetQuestionByIdQuery request, CancellationToken ct)
        {
            var questionDTO = await _questionRepo
                .Get(q => q.Id == request.QuestionId)
                .Select(q => new QuestionForEditDto(
                    q.Id,
                    q.Text,
                    q.Explanation,
                    q.Quiz!.Status
                )).FirstOrDefaultAsync(ct);

            if (questionDTO == null)
                return RequestResult<QuestionForEditDto>.Failure(
                    "Question not found", RequestErrorCode.NotFound);

            return RequestResult<QuestionForEditDto>.Success(questionDTO);
        }
    }
}

