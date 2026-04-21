namespace ExaminationSystem.Features.QuizModule.PublishQuiz.Requests
{
    public record CheckQuizPublishStateQueryRequest(Guid quizId) : IRequest<RequestResult<bool>>;

    public class CheckQuizPublishStateQueryRequestValidator : AbstractValidator<CheckQuizPublishStateQueryRequest>
    {
        public CheckQuizPublishStateQueryRequestValidator()
        {
            RuleFor(r => r.quizId)
                .NotEmpty()
                .WithMessage("Quiz Id is required.");
        }
    }

    public class CheckQuizPublishStateQueryRequestHandler : IRequestHandler<CheckQuizPublishStateQueryRequest, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Quiz> _quizzesRepository;
        private readonly IValidator<CheckQuizPublishStateQueryRequest> _validator;
        public CheckQuizPublishStateQueryRequestHandler(IGeneralRepository<Quiz> quizzesRepository, IValidator<CheckQuizPublishStateQueryRequest> validator)
        {
            _quizzesRepository = quizzesRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(CheckQuizPublishStateQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
                .ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string
                    .Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<bool>
                    .Failure(validationErrors, RequestErrorCode.ValidationError);
            }

            bool isQuizPublished = await _quizzesRepository
                .Get(q => q.Id == request.quizId && q.Status == QuizStatus.Published)
                .AnyAsync(cancellationToken);

            return RequestResult<bool>.Success(isQuizPublished);
        }
    }
}
