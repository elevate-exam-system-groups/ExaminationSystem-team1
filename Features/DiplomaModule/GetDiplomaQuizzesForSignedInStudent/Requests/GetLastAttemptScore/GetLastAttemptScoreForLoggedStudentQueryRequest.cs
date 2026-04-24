
namespace ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzesForSignedInStudent.Requests.GetLastAttemptScore
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

        public GetLastAttemptScoreForLoggedStudentQueryRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptsRepository)
        {
            _quizAttemptsRepository = quizAttemptsRepository;
        }

        public async Task<RequestResult<decimal?>> Handle(GetLastAttemptScoreForLoggedStudentQueryRequest request, CancellationToken cancellationToken)
        {
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
