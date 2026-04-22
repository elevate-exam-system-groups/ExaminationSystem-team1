using ExaminationSystem.Features.QuizModule.SubmitQuiz.Requests.Queries.GetAttemptResult.DTO;

namespace ExaminationSystem.Features.QuizModule.SubmitQuiz.Requests.Queries.GetAttemptResult
{
    public record GetAttemptScoreQueryRequest(Guid attemptId) : IRequest<RequestResult<GetAttemptResultDTO>>;


    public class GetAttemptScoreQueryRequestValidator : AbstractValidator<GetAttemptScoreQueryRequest>
    {
        public GetAttemptScoreQueryRequestValidator()
        {
            RuleFor(x => x.attemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
        }
    }

    public class GetAttemptScoreQueryRequestHandler
        : IRequestHandler<GetAttemptScoreQueryRequest, RequestResult<GetAttemptResultDTO>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<GetAttemptScoreQueryRequest> _validator;
        public GetAttemptScoreQueryRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<GetAttemptScoreQueryRequest> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<GetAttemptResultDTO>> Handle(GetAttemptScoreQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                var result = RequestResult<GetAttemptResultDTO>
                                            .Failure(validationErrors, RequestErrorCode.ValidationError);
                return result;
            }

            var attemptData = _quizAttemptsRepository
             .Get(qa => qa.Id == request.attemptId)
             .Select(qa => new
             {
                 TotalQuestions = qa.Quiz.Questions.Count,
                 CorrectAnswers = qa.UserAnswers
                  .Count(a => a.SelectedOption.IsCorrect),
                 qa.Quiz.PassScore
             }).FirstOrDefault();

            if (attemptData is null || attemptData.TotalQuestions == 0)
            {
                return RequestResult<GetAttemptResultDTO>
                    .Failure("Attempt not found or quiz has no questions", RequestErrorCode.NotFound);
            }

            decimal score = (decimal)attemptData.CorrectAnswers / attemptData.TotalQuestions * 100;

            bool isPassed = score >= attemptData.PassScore;
            var resultDTO = new GetAttemptResultDTO(score, isPassed);

            return RequestResult<GetAttemptResultDTO>.Success(resultDTO);
        }
    }
}
