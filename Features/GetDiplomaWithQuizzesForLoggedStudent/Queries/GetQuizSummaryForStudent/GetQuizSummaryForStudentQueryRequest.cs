using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.GetDiplomaWithQuizzesForLoggedStudent.Queries.GetQuizSummaryForStudent.DTOS;

namespace ExaminationSystem.Features.GetDiplomaWithQuizzesForLoggedStudent.Queries.GetQuizSummaryForStudent
{
    public record GetQuizSummaryForStudentQueryRequest(Guid QuizId, string StudentId, int? MaxAttempts)
        : IRequest<RequestResult<GetQuizSummaryDTO>>;

    public class GetQuizSummaryForStudentQueryRequestValidator
        : AbstractValidator<GetQuizSummaryForStudentQueryRequest>
    {
        public GetQuizSummaryForStudentQueryRequestValidator()
        {
            RuleFor(x => x.QuizId)
                .NotEmpty().WithMessage("QuizId is required");
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("StudentId is required");
            RuleFor(x => x.MaxAttempts)
                .GreaterThan(0).WithMessage("MaxAttempts must be greater than 0");
        }
    }

    public class GetQuizSummaryForStudentQueryRequestHandler
    : IRequestHandler<GetQuizSummaryForStudentQueryRequest, RequestResult<GetQuizSummaryDTO>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptRepository;
        private readonly IValidator<GetQuizSummaryForStudentQueryRequest> _validator;

        public GetQuizSummaryForStudentQueryRequestHandler(
            IGeneralRepository<QuizAttempt> quizAttemptRepository,
            IValidator<GetQuizSummaryForStudentQueryRequest> validator)
        {
            _quizAttemptRepository = quizAttemptRepository;
            _validator = validator;
        }

        public async Task<RequestResult<GetQuizSummaryDTO>> Handle(
            GetQuizSummaryForStudentQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator
             .ValidateRequestAsync<GetQuizSummaryForStudentQueryRequest, GetQuizSummaryDTO>(request, cancellationToken);

            if (!validationResult.IsSuccess)
                return validationResult;

            var attempts = _quizAttemptRepository
                .Get(qa => qa.StudentId == request.StudentId && qa.QuizId == request.QuizId)
                .OrderByDescending(qa => qa.SubmittedAt)
                .ToList();

            var attemptCount = attempts.Count;
            var canAttempt = request.MaxAttempts == null || attemptCount < request.MaxAttempts;
            var lastScore = attempts.FirstOrDefault()?.Score;

            return RequestResult<GetQuizSummaryDTO>.Success(
                new GetQuizSummaryDTO(attemptCount, canAttempt, lastScore)
            );
        }
    }
}