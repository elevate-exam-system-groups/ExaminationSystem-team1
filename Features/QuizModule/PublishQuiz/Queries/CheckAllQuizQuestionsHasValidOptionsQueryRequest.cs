namespace ExaminationSystem.Features.QuizModule.PublishQuiz.Queries
{
    public record CheckAllQuizQuestionsHasValidOptionsQueryRequest(Guid quizId) : IRequest<RequestResult<bool>>;

    public class CheckAllQuizQuestionsHasValidOptionsQueryRequestValidator : AbstractValidator<CheckAllQuizQuestionsHasValidOptionsQueryRequest>
    {
        public CheckAllQuizQuestionsHasValidOptionsQueryRequestValidator()
        {
            RuleFor(r => r.quizId)
                .NotEmpty()
                .WithMessage("Quiz Id is required.");
        }
    }

    public class CheckAllQuizQuestionsHasValidOptionsQueryRequestHandler
        : IRequestHandler<CheckAllQuizQuestionsHasValidOptionsQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Question> _questionRepository;
        private readonly IValidator<CheckAllQuizQuestionsHasValidOptionsQueryRequest> _validator;

        public CheckAllQuizQuestionsHasValidOptionsQueryRequestHandler(IGeneralRepository<Question> questionRepository, IValidator<CheckAllQuizQuestionsHasValidOptionsQueryRequest> validator)
        {
            _questionRepository = questionRepository;
            _validator = validator;
        }

        public async Task<RequestResult<bool>> Handle(CheckAllQuizQuestionsHasValidOptionsQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
              .ValidateRequestAsync<CheckAllQuizQuestionsHasValidOptionsQueryRequest, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var HasInValidQuestions = await _questionRepository
                 .Get(q => q.QuizId == request.quizId)
                 .AnyAsync(q => q.Options == null ||
                                  q.Options.Count < 2 ||
                                  !q.Options.Any(o => o.IsCorrect));

            if (HasInValidQuestions)
            {
                return RequestResult<bool>
                    .Failure("All questions must have at least 2 options and at least one correct option.", RequestErrorCode.Forbidden);
            }

            return RequestResult<bool>.Success(true);
        }
    }


}
