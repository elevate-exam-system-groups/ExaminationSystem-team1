using ExaminationSystem.Features.Common.FeatureExtensions;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Common.QuizRequests.Queries
{
    public record GetQuizMaxAttemptsQuery(Guid quizId) : IRequest<RequestResult<int?>>;

    public class GetQuizMaxAttemptsQueryValidator : AbstractValidator<GetQuizMaxAttemptsQuery>
    {
        public GetQuizMaxAttemptsQueryValidator()
        {
            RuleFor(r => r.quizId)
                .NotEmpty()
                .WithMessage("Quiz Id is required.");
        }
    }

    public class GetQuizMaxAttemptsQueryHandler : IRequestHandler<GetQuizMaxAttemptsQuery, RequestResult<int?>>
    {
        private readonly IGeneralRepository<Quiz> _quizRepository;
        private readonly IValidator<GetQuizMaxAttemptsQuery> _validator;
        public GetQuizMaxAttemptsQueryHandler(IGeneralRepository<Quiz> quizzesRepository, IValidator<GetQuizMaxAttemptsQuery> validator)
        {
            _quizRepository = quizzesRepository;
            _validator = validator;
        }
        public async Task<RequestResult<int?>> Handle(GetQuizMaxAttemptsQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
               .ValidateRequestAsync<GetQuizMaxAttemptsQuery, int?>(request, cancellationToken);
            if (!validationResult.IsSuccess)
                return validationResult;

            var maxAttempts = await _quizRepository
                .Get(q => q.Id == request.quizId)
                .Select(q => q.MaxAttempts)
                .FirstOrDefaultAsync(cancellationToken);

            return RequestResult<int?>.Success(maxAttempts);
        }
    }
}
