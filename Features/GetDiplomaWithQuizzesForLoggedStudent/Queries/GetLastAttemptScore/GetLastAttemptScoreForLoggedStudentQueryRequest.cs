using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.GetDiplomaWithQuizzesForLoggedStudent.Queries.GetLastAttemptScore
{
    public record GetLastAttemptScoreForLoggedStudentQueryRequest(Guid quizId, string StudentId) : IRequest<RequestResult<decimal?>>;

    public class GetLastAttemptScoreForLoggedStudentQueryRequestValidator
        : AbstractValidator<GetLastAttemptScoreForLoggedStudentQueryRequest>
    {
        public GetLastAttemptScoreForLoggedStudentQueryRequestValidator()
        {
            RuleFor(x => x.quizId)
                .NotEmpty().WithMessage("QuizId is required");

            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("StudentId is required");
        }
    }

    public class GetLastAttemptScoreForLoggedStudentQueryRequestHandler
        : IRequestHandler<GetLastAttemptScoreForLoggedStudentQueryRequest, RequestResult<decimal?>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptsRepository;
        private readonly IValidator<GetLastAttemptScoreForLoggedStudentQueryRequest> _validator;

        public GetLastAttemptScoreForLoggedStudentQueryRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository, IValidator<GetLastAttemptScoreForLoggedStudentQueryRequest> validator)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
            _validator = validator;
        }

        public async Task<RequestResult<decimal?>> Handle(GetLastAttemptScoreForLoggedStudentQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
             .ValidateRequestAsync<GetLastAttemptScoreForLoggedStudentQueryRequest, decimal?>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var lastAttempt = _quizAttemptsRepository
                .Get(qa => qa.StudentId == request.StudentId && qa.QuizId == request.quizId)
                .OrderByDescending(qa => qa.SubmittedAt)
                .FirstOrDefault();

            if (lastAttempt == null)
            {
                return RequestResult<decimal?>.Success(null);
            }

            return RequestResult<decimal?>.Success(lastAttempt.Score);
        }
    }
}
