namespace ExaminationSystem.Features.QuizModule.PublishQuiz.Queries
{
    public record GetQuestionsCountPerQuizQueryRequest(Guid quizId) : IRequest<RequestResult<int>>;

    public class GetQuestionsCountPerQuizQueryRequestValidator : AbstractValidator<GetQuestionsCountPerQuizQueryRequest>
    {
        public GetQuestionsCountPerQuizQueryRequestValidator()
        {
            RuleFor(r => r.quizId)
                .NotEmpty()
                .WithMessage("Quiz Id is required.");
        }
    }

    public class GetQuestionsCountPerQuizQueryRequestHandler : IRequestHandler<GetQuestionsCountPerQuizQueryRequest, RequestResult<int>>
    {
        private readonly IGeneralRepository<Question> _questionsRepository;
        private readonly IValidator<GetQuestionsCountPerQuizQueryRequest> _validator;
        public GetQuestionsCountPerQuizQueryRequestHandler(IGeneralRepository<Question> questionsRepository, IValidator<GetQuestionsCountPerQuizQueryRequest> validator)
        {
            _questionsRepository = questionsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<int>> Handle(GetQuestionsCountPerQuizQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
                .ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var validationErrors = string
                    .Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<int>
                    .Failure(validationErrors, RequestErrorCode.ValidationError);
            }

            int questionsCount = await _questionsRepository
                .Get(q => q.QuizId == request.quizId)
                .CountAsync(cancellationToken);

            return RequestResult<int>.Success(questionsCount);
        }
    }
}
