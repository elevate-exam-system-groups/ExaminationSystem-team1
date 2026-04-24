using ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzesForSignedInStudent.DTOS;
using System.Security.Claims;

namespace ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzesForSignedInStudent
{
    public record GetDiplomaQuizzesForSignedInStudentQueryRequest(Guid DiplomaId) : IRequest<RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>>;

    public class GetDiplomaQuizzesForSignedInStudentQueryRequestValidator : AbstractValidator<GetDiplomaQuizzesForSignedInStudentQueryRequest>
    {
        public GetDiplomaQuizzesForSignedInStudentQueryRequestValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }

    public class GetDiplomaQuizzesForSignedInStudentQueryRequestHandler : IRequestHandler<GetDiplomaQuizzesForSignedInStudentQueryRequest, RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IGeneralRepository<Quiz> _quizRepository;
        private readonly IGeneralRepository<Quiz> _studentQuizAttemptRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<GetDiplomaQuizzesForSignedInStudentQueryRequest> _validator;


        public GetDiplomaQuizzesForSignedInStudentQueryRequestHandler(IGeneralRepository<Diploma> diplomaRepository, IGeneralRepository<Quiz> quizRepository, IGeneralRepository<Quiz> studentQuizAttemptRepository, IHttpContextAccessor httpContextAccessor)
        {
            _diplomaRepository = diplomaRepository;
            _quizRepository = quizRepository;
            _studentQuizAttemptRepository = studentQuizAttemptRepository;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>> Handle(GetDiplomaQuizzesForSignedInStudentQueryRequest request, CancellationToken cancellationToken)
        {
            var diploma = _diplomaRepository
                .Get(d => d.Id == request.DiplomaId && d.Status == DiplomaStatus.Published);

            if (diploma == null || !diploma.Any())
            {
                return RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>
                    .Failure("Diploma not found or not published", RequestErrorCode.NotFound);
            }
            var studentID = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool isEnrolled = diploma.Any(d => d.Enrollments.Any(e => e.StudentId == studentID));

            if (!isEnrolled)
            {
                return RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>
                    .Failure("Student is not enrolled in this diploma", RequestErrorCode.Forbidden);
            }

            var Quizzes = _quizRepository
                .Get(q => q.DiplomaId == request.DiplomaId && q.Status == QuizStatus.Published);

            var attemptsQuery = _studentQuizAttemptRepository
                .Get(qa => qa.StudentId == studentID);

            var response = await Quizzes.Select(q => new GetDiplomaQuizzesForLoggedStudentDTO
            (
               q.Id,
               q.Title,
               attemptsQuery.Count(qa => qa.QuizId == q.Id),
               q.DurationInMinutes,
               q.PassScore,
               q.MaxAttempts,
               q.MaxAttempts == null ||
                  attemptsQuery.Count(qa => qa.QuizId == q.Id) < q.MaxAttempts, // canAttempt 
               attemptsQuery
                  .Where(qa => qa.QuizId == q.Id)
                  .OrderByDescending(qa => qa.SubmittedAt)
                  .Select(qa => qa.Score)
                  .FirstOrDefault(), // lastScore per quiz
               q.Status
            )).ToListAsync(cancellationToken);


            return RequestResult<List<GetDiplomaQuizzesForLoggedStudentDTO>>.Success(response);
        }
    }
}


