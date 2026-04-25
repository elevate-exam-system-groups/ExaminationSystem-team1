using ExaminationSystem.Features.Common.FeatureExtensions;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Common.QuizRequests.Queries
{
    public record IsQuizPublishedQuery(Guid quizId) : IRequest<RequestResult<bool>>;

    public class IsQuizPublishedQueryValidator : AbstractValidator<IsQuizPublishedQuery>
    {
        public IsQuizPublishedQueryValidator()
        {
            RuleFor(r => r.quizId)
                .NotEmpty()
                .WithMessage("Quiz Id is required.");
        }
    }

    public class IsQuizPublishedQueryHandler : IRequestHandler<IsQuizPublishedQuery, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Quiz> _quizzesRepository;
        private readonly IValidator<IsQuizPublishedQuery> _validator;
        public IsQuizPublishedQueryHandler(IGeneralRepository<Quiz> quizzesRepository, IValidator<IsQuizPublishedQuery> validator)
        {
            _quizzesRepository = quizzesRepository;
            _validator = validator;
        }
        public async Task<RequestResult<bool>> Handle(IsQuizPublishedQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
             .ValidateRequestAsync<IsQuizPublishedQuery, bool>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            bool isQuizPublished = await _quizzesRepository
                .Get(q => q.Id == request.quizId && q.Status == QuizStatus.Published)
                .AnyAsync(cancellationToken);

            return RequestResult<bool>.Success(isQuizPublished);
        }
    }
}
