namespace ExaminationSystem.Features.AnswerQuestion.Requests
{
    public record CheckQuestionBelongsToQuizQueryRequest(Guid questionId, Guid quizId)
        : IRequest<RequestResult<bool>>;

    public class CheckQuestionBelongsToQuizQueryRequestValidator : AbstractValidator<CheckQuestionBelongsToQuizQueryRequest>
    {
        public CheckQuestionBelongsToQuizQueryRequestValidator()
        {
            RuleFor(x => x.questionId)
                .NotEmpty().WithMessage("Question ID is required");
            RuleFor(x => x.quizId)
                .NotEmpty().WithMessage("Quiz ID is required");
        }
    }

    public class CheckQuestionBelongsToQuizQueryRequestHandler
        : IRequestHandler<CheckQuestionBelongsToQuizQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Question> _questionsRepository;
        private readonly IValidator<CheckQuestionBelongsToQuizQueryRequest> _validator;
        public CheckQuestionBelongsToQuizQueryRequestHandler(IGeneralRepository<Question> questionsRepository, IValidator<CheckQuestionBelongsToQuizQueryRequest> validator)
        {
            _questionsRepository = questionsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(CheckQuestionBelongsToQuizQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                var result = RequestResult<bool>
                                            .Failure(validationErrors, RequestErrorCode.ValidationError);
                return result;
            }
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
