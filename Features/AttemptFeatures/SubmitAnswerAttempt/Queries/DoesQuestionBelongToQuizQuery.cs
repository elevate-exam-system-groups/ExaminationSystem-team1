using ExaminationSystem.Features.Common.FeatureExtensions;

namespace ExaminationSystem.Features.AttemptFeatures.SubmitAnswerAttempt.Queries
{
    public record DoesQuestionBelongToQuizQuery(Guid questionId, Guid quizId)
        : IRequest<RequestResult<bool>>;

    public class DoesQuestionBelongToQuizQueryValidator : AbstractValidator<DoesQuestionBelongToQuizQuery>
    {
        public DoesQuestionBelongToQuizQueryValidator()
        {
            RuleFor(x => x.questionId)
                .NotEmpty().WithMessage("Question ID is required");
            RuleFor(x => x.quizId)
                .NotEmpty().WithMessage("Quiz ID is required");
        }
    }

    public class DoesQuestionBelongToQuizQueryHandler
        : IRequestHandler<DoesQuestionBelongToQuizQuery, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Question> _questionsRepository;
        private readonly IValidator<DoesQuestionBelongToQuizQuery> _validator;
        public DoesQuestionBelongToQuizQueryHandler(IGeneralRepository<Question> questionsRepository, IValidator<DoesQuestionBelongToQuizQuery> validator)
        {
            _questionsRepository = questionsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(DoesQuestionBelongToQuizQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
              .ValidateRequestAsync<DoesQuestionBelongToQuizQuery, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;


            var isQuestionPartOfQuiz = _questionsRepository
               .Get(q => q.Id == request.questionId && q.QuizId == request.quizId)
               .Any();

            if (!isQuestionPartOfQuiz)
            {
                return RequestResult<bool>
                    .Failure("Question is not part of the quiz", RequestErrorCode.UnprocessableEntity);
            }

            return RequestResult<bool>.Success(isQuestionPartOfQuiz);
        }
    }
}
