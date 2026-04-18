using ExaminationSystem.Features.DiplomaModule.ViewDiplomaQuizzes.DTOS;
using System.Security.Claims;

namespace ExaminationSystem.Features.DiplomaModule.ViewDiplomaQuizzes
{
    public record ViewDiplomaQuizzesQueryRequest(Guid DiplomaId) : IRequest<RequestResult<List<ViewDiplomaQuizzesResponseDTO>>>;

    public class ViewDiplomaQuizzesQueryRequestValidator : AbstractValidator<ViewDiplomaQuizzesQueryRequest>
    {
        public ViewDiplomaQuizzesQueryRequestValidator()
        {
            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }

    public class ViewDiplomaQuizzesQueryRequestHandler : IRequestHandler<ViewDiplomaQuizzesQueryRequest, RequestResult<List<ViewDiplomaQuizzesResponseDTO>>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IGeneralRepository<Quiz> _quizRepository;
        private readonly IGeneralRepository<QuizAttempt> _studentQuizAttemptRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<ViewDiplomaQuizzesQueryRequest> _validator;


        public ViewDiplomaQuizzesQueryRequestHandler(IGeneralRepository<Diploma> diplomaRepository, IGeneralRepository<Quiz> quizRepository, IGeneralRepository<QuizAttempt> studentQuizAttemptRepository, IHttpContextAccessor httpContextAccessor)
        {
            _diplomaRepository = diplomaRepository;
            _quizRepository = quizRepository;
            _studentQuizAttemptRepository = studentQuizAttemptRepository;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<RequestResult<List<ViewDiplomaQuizzesResponseDTO>>> Handle(ViewDiplomaQuizzesQueryRequest request, CancellationToken cancellationToken)
        {
            var diploma = _diplomaRepository
                .Get(d => d.Id == request.DiplomaId && d.Status == DiplomaStatus.Published);

            if (diploma == null || !diploma.Any())
            {
                return RequestResult<List<ViewDiplomaQuizzesResponseDTO>>
                    .Failure("Diploma not found or not published", RequestErrorCode.NotFound);
            }
            var studentID = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool isEnrolled = diploma.Any((d => d.Enrollments.Any(e => e.StudentId == studentID)));

            if (!isEnrolled)
            {
                return RequestResult<List<ViewDiplomaQuizzesResponseDTO>>
                    .Failure("Student is not enrolled in this diploma", RequestErrorCode.Forbidden);
            }

            var Quizzes = _quizRepository
                .Get(q => q.DiplomaId == request.DiplomaId && q.Status == QuizStatus.Published);

            //var quizID = Quizzes.Select(q => q.Id)   ;

            var attemptsQuery = _studentQuizAttemptRepository
                .Get(qa => qa.StudentId == studentID);

            //var lastScore = _studentQuizAttemptRepository
            //    .Get(qa => qa.StudentId == studentID && qa.QuizId == quizID.FirstOrDefault())
            //    .OrderByDescending(qa => qa.SubmittedAt)
            //    .Select(qa => qa.Score)
            //    .FirstOrDefault();

            var response = await Quizzes.Select(q => new ViewDiplomaQuizzesResponseDTO
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


            return RequestResult<List<ViewDiplomaQuizzesResponseDTO>>.Success(response);
        }
    }
}


