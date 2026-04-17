namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Query
{
    public class GetQuestionForEditQueryHandler
     : IRequestHandler<GetQuestionForEditQuery, RequestResult<QuestionForEditDto>>
    {

        private readonly IGeneralRepository<Question> _questionRepo;
        public GetQuestionForEditQueryHandler(IGeneralRepository<Question> questionRepo)
            => _questionRepo = questionRepo;

        public async Task<RequestResult<QuestionForEditDto>> Handle(
            GetQuestionForEditQuery request, CancellationToken ct)
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

            if (questionDTO.QuizStatus == QuizStatus.Published)
                return RequestResult<QuestionForEditDto>.Failure(
                    "Cannot update question in a published quiz. Unpublish quiz first.",
                    RequestErrorCode.Conflict);

            return RequestResult<QuestionForEditDto>.Success(questionDTO);
        }
    }
}

