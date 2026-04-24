using ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzesForSignedInStudent.Requests.GetQuizSummaryForStudent.DTOS;

namespace ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzesForSignedInStudent.Requests.GetQuizSummaryForStudent
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
        private readonly IGeneralRepository<Quiz> _quizAttemptRepository;
        private readonly IValidator<GetQuizSummaryForStudentQueryRequest> _validator;

        public GetQuizSummaryForStudentQueryRequestHandler(
            IGeneralRepository<Quiz> quizAttemptRepository,
            IValidator<GetQuizSummaryForStudentQueryRequest> validator)
        {
            _quizAttemptRepository = quizAttemptRepository;
            _validator = validator;
        }

        public async Task<RequestResult<GetQuizSummaryDTO>> Handle(
            GetQuizSummaryForStudentQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ",
                    validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<GetQuizSummaryDTO>
                    .Failure(validationErrors, RequestErrorCode.ValidationError);

            }

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