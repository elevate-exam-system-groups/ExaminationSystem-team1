namespace ExaminationSystem.Features.StudentDashboard.Queries.GetCompletedQuizIds
{
    public class GetCompletedQuizIdsQueryHandler
     : IRequestHandler<GetCompletedQuizIdsQuery, RequestResult<List<Guid>>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetCompletedQuizIdsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<List<Guid>>> Handle(
            GetCompletedQuizIdsQuery request, CancellationToken ct)
        {
            if (!request.DiplomaIds.Any())
                return RequestResult<List<Guid>>.Success(new());

            var quizIds = await _attemptRepo
                .Get(a => a.StudentId == request.StudentId
                       && request.DiplomaIds.Contains(a.Quiz.DiplomaId)
                       && a.Status != QuizAttemptStatus.InProgress
                       && !a.isDeleted)
                .Select(a => a.QuizId)
                .Distinct()
                .ToListAsync(ct);

            return RequestResult<List<Guid>>.Success(quizIds);
        }
    }
}

