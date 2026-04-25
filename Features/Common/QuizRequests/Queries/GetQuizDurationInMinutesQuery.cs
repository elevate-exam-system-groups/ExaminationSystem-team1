using ExaminationSystem.Features.Common.FeatureExtensions;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Common.QuizRequests.Queries
{
    public record GetQuizDurationInMinutesQuery(Guid quizId) : IRequest<RequestResult<int>>;

    public class GetQuizDurationInMinutesQueryValidator : AbstractValidator<GetQuizDurationInMinutesQuery>
    {
        public GetQuizDurationInMinutesQueryValidator()
        {
            RuleFor(r => r.quizId)
                .NotEmpty()
                .WithMessage("Quiz Id is required.");
        }
    }

    public class GetQuizDurationInMinutesQueryHandler : IRequestHandler<GetQuizDurationInMinutesQuery, RequestResult<int>>
    {
        private readonly IGeneralRepository<Quiz> _quizzesRepository;
        private readonly IValidator<GetQuizDurationInMinutesQuery> _validator;
        public GetQuizDurationInMinutesQueryHandler(IGeneralRepository<Quiz> quizzesRepository, IValidator<GetQuizDurationInMinutesQuery> validator)
        {
            _quizzesRepository = quizzesRepository;
            _validator = validator;
        }
        public async Task<RequestResult<int>> Handle(GetQuizDurationInMinutesQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
               .ValidateRequestAsync<GetQuizDurationInMinutesQuery, int>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var duration = await _quizzesRepository
                .Get(q => q.Id == request.quizId)
                .Select(q => (int?)q.DurationInMinutes)
                .FirstOrDefaultAsync(cancellationToken);

            if (duration is null)
            {
                return RequestResult<int>
                    .Failure("Quiz not found.", RequestErrorCode.NotFound);
            }

            return RequestResult<int>.Success(duration.Value);
        }
    }

}
