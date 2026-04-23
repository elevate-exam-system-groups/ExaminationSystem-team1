using ExaminationSystem.Features.Attempts.SubmitQuizAttempt.Requests.Queries.GetAttemptResult.DTO;

namespace ExaminationSystem.Features.Attempts.SubmitQuizAttempt.Requests.Queries.GetAttemptResult
{
    public record GetAttemptResultQuery(Guid attemptId) : IRequest<RequestResult<AttemptResultDTO>>;


    public class GetAttemptResultQueryValidator : AbstractValidator<GetAttemptResultQuery>
    {
        public GetAttemptResultQueryValidator()
        {
            RuleFor(x => x.attemptId)
                .NotEmpty().WithMessage("Attempt ID is required");
        }
    }

    public class GetAttemptResultQueryHandler
        : IRequestHandler<GetAttemptResultQuery, RequestResult<AttemptResultDTO>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<GetAttemptResultQuery> _validator;
        public GetAttemptResultQueryHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<GetAttemptResultQuery> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<AttemptResultDTO>> Handle(GetAttemptResultQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                var result = RequestResult<AttemptResultDTO>
                                            .Failure(validationErrors, RequestErrorCode.ValidationError);
                return result;
            }

            var attemptData = await _quizAttemptsRepository
             .Get(qa => qa.Id == request.attemptId)
             .Select(qa => new
             {
                 TotalQuestions = qa.Quiz.Questions.Count(),
                 CorrectAnswers = qa.UserAnswers
                  .Count(a => a.SelectedOption.IsCorrect),
                 qa.Quiz.PassScore
             }).FirstOrDefaultAsync();

            if (attemptData is null || attemptData.TotalQuestions == 0)
            {
                return RequestResult<AttemptResultDTO>
                    .Failure("Attempt not found or quiz has no questions", RequestErrorCode.NotFound);
            }

            decimal score = (decimal)attemptData.CorrectAnswers / attemptData.TotalQuestions * 100;

            bool isPassed = score >= attemptData.PassScore;
            var resultDTO = new AttemptResultDTO(score, isPassed);

            return RequestResult<AttemptResultDTO>.Success(resultDTO);
        }
    }
}
