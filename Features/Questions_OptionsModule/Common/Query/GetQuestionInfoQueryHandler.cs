using ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.Common.Query
{
    public class GetQuestionInfoQueryHandler
          : IRequestHandler<GetQuestionInfoQuery, RequestResult<QuestionInfoDto>>
    {
        private readonly IGeneralRepository<Question> _questionRepo;

        public GetQuestionInfoQueryHandler(IGeneralRepository<Question> questionRepo)
        {
            _questionRepo = questionRepo;
        }

        public async Task<RequestResult<QuestionInfoDto>> Handle(
            GetQuestionInfoQuery request, CancellationToken ct)
        {
            var questionInfo = await _questionRepo
                .Get(q => q.Id == request.QuestionId && !q.isDeleted)
                .Select(q => new QuestionInfoDto(
                    q.Id,
                    q.QuizId,
                    q.Quiz != null ? q.Quiz.Status : QuizStatus.Draft
                ))
                .FirstOrDefaultAsync(ct);

            if (questionInfo == null)
            {
                return RequestResult<QuestionInfoDto>.Failure(
                    "Question not found",
                    RequestErrorCode.NotFound);
            }

            // Business Rule: Cannot delete from published quiz
            if (questionInfo.QuizStatus == QuizStatus.Published)
            {
                return RequestResult<QuestionInfoDto>.Failure(
                    "Cannot delete question from a published quiz. Unpublish quiz first.",
                    RequestErrorCode.Conflict);
            }

            return RequestResult<QuestionInfoDto>.Success(questionInfo);
        }
    }
}