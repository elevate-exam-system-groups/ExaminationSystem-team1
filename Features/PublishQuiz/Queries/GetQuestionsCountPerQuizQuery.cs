using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.PublishQuiz.Queries
{
    public record GetQuestionsCountPerQuizQuery(Guid quizId) : IRequest<RequestResult<int>>;

    public class GetQuestionsCountPerQuizQueryValidator : AbstractValidator<GetQuestionsCountPerQuizQuery>
    {
        public GetQuestionsCountPerQuizQueryValidator()
        {
            RuleFor(r => r.quizId)
                .NotEmpty()
                .WithMessage("Quiz Id is required.");
        }
    }

    public class GetQuestionsCountPerQuizQueryHandler : IRequestHandler<GetQuestionsCountPerQuizQuery, RequestResult<int>>
    {
        private readonly IGeneralRepository<Question> _questionsRepository;
        private readonly IValidator<GetQuestionsCountPerQuizQuery> _validator;
        public GetQuestionsCountPerQuizQueryHandler(IGeneralRepository<Question> questionsRepository, IValidator<GetQuestionsCountPerQuizQuery> validator)
        {
            _questionsRepository = questionsRepository;
            _validator = validator;
        }
        public async Task<RequestResult<int>> Handle(GetQuestionsCountPerQuizQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
              .ValidateRequestAsync<GetQuestionsCountPerQuizQuery, int>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            int questionsCount = await _questionsRepository
                .Get(q => q.QuizId == request.quizId)
                .CountAsync(cancellationToken);

            return RequestResult<int>.Success(questionsCount);
        }
    }
}
