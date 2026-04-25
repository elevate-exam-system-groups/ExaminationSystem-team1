using ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent.Queries.GetStudentQuizzesAttemptsForDiploma.DTOS;

namespace ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent.Queries.GetStudentQuizzesAttemptsForDiploma
{
    public record GetStudentQuizzesAttemptsForDiplomaQueryRequest(Guid DiplomaId, string StudentId)
        : IRequest<RequestResult<List<GetQuizAttemptsDTO>>>;

    public class GetStudentQuizzesAttemptsForDiplomaQueryRequestValidator
        : AbstractValidator<GetStudentQuizzesAttemptsForDiplomaQueryRequest>
    {
        public GetStudentQuizzesAttemptsForDiplomaQueryRequestValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("StudentId is required");
        }
    }

    public class GetStudentQuizzesAttemptsForDiplomaQueryRequestHandler : IRequestHandler<GetStudentQuizzesAttemptsForDiplomaQueryRequest, RequestResult<List<GetQuizAttemptsDTO>>>
    {
        private readonly IGeneralRepository<QuizAttempt> _quizAttemptRepository;
        private readonly IValidator<GetStudentQuizzesAttemptsForDiplomaQueryRequest> _validator;
        public GetStudentQuizzesAttemptsForDiplomaQueryRequestHandler(IGeneralRepository<QuizAttempt> quizAttemptRepository, IValidator<GetStudentQuizzesAttemptsForDiplomaQueryRequest> validator)
        {
            _quizAttemptRepository = quizAttemptRepository;
            _validator = validator;
        }
        public async Task<RequestResult<List<GetQuizAttemptsDTO>>> Handle(GetStudentQuizzesAttemptsForDiplomaQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return RequestResult<List<GetQuizAttemptsDTO>>
                .Failure(validationResult.Errors.First().ErrorMessage);
            }

            var AttemptCount = _quizAttemptRepository
                .Get(qa => qa.StudentId == request.StudentId && qa.Quiz.DiplomaId == request.DiplomaId)
                .Count();

            var quizAttempts = _quizAttemptRepository
                .Get(qa => qa.StudentId == request.StudentId && qa.Quiz.DiplomaId == request.DiplomaId)
                .Select(qa => new GetQuizAttemptsDTO
                (
                    qa.Id,
                    qa.QuizId,
                    qa.Score,
                    qa.Status,
                    qa.SubmittedAt,
                    AttemptCount
                )).ToList();

            if (quizAttempts == null || !quizAttempts.Any())
            {
                return RequestResult<List<GetQuizAttemptsDTO>>
                .Failure("No quiz attempts found for the specified diploma and student");
            }
            return RequestResult<List<GetQuizAttemptsDTO>>
                .Success(quizAttempts);
        }
    }

}
