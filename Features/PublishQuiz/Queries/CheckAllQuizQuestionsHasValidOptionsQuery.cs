using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.PublishQuiz.Queries
{
    public record CheckAllQuizQuestionsHasValidOptionsQuery(Guid quizId) : IRequest<RequestResult<bool>>;

    public class CheckAllQuizQuestionsHasValidOptionsQueryValidator : AbstractValidator<CheckAllQuizQuestionsHasValidOptionsQuery>
    {
        public CheckAllQuizQuestionsHasValidOptionsQueryValidator()
        {
            RuleFor(r => r.quizId)
                .NotEmpty()
                .WithMessage("Quiz Id is required.");
        }
    }

    public class CheckAllQuizQuestionsHasValidOptionsQueryHandler
        : IRequestHandler<CheckAllQuizQuestionsHasValidOptionsQuery, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Question> _questionRepository;
        private readonly IValidator<CheckAllQuizQuestionsHasValidOptionsQuery> _validator;

        public CheckAllQuizQuestionsHasValidOptionsQueryHandler(IGeneralRepository<Question> questionRepository, IValidator<CheckAllQuizQuestionsHasValidOptionsQuery> validator)
        {
            _questionRepository = questionRepository;
            _validator = validator;
        }

        public async Task<RequestResult<bool>> Handle(CheckAllQuizQuestionsHasValidOptionsQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
              .ValidateRequestAsync<CheckAllQuizQuestionsHasValidOptionsQuery, bool>(request, cancellationToken);

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
