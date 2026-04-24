using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.SubmitAnswerAttempt.Queries
{
    public record GetQuizIdByQuestionIdQuery(Guid QuestionId)
        : IRequest<RequestResult<Guid>>;

    public class GetQuizIdByQuestionIdQueryValidator : AbstractValidator<GetQuizIdByQuestionIdQuery>
    {
        public GetQuizIdByQuestionIdQueryValidator()
        {
            RuleFor(q => q.QuestionId).NotEmpty();
        }
    }

    public class GetQuizIdByQuestionIdQueryHandler
        : IRequestHandler<GetQuizIdByQuestionIdQuery, RequestResult<Guid>>
    {
        private readonly IGeneralRepository<Question> _questionRepository;
        private readonly IValidator<GetQuizIdByQuestionIdQuery> _validator;

        public GetQuizIdByQuestionIdQueryHandler(IGeneralRepository<Question> questionRepository, IValidator<GetQuizIdByQuestionIdQuery> validator)
        {
            _questionRepository = questionRepository;
            _validator = validator;
        }

        public async Task<RequestResult<Guid>> Handle(GetQuizIdByQuestionIdQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
              .ValidateRequestAsync<GetQuizIdByQuestionIdQuery, Guid>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            Guid quizId = await _questionRepository
                .Get(qu => qu.Id == request.QuestionId)
                .Select(qu => qu.QuizId).FirstOrDefaultAsync();

            if (quizId == Guid.Empty)
                return RequestResult<Guid>
                    .Failure("Question not found", RequestErrorCode.NotFound);

            return RequestResult<Guid>.Success(quizId);
        }
    }
}
