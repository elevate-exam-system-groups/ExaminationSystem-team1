using ExaminationSystem.Features.StudentDashboard.Helper;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetTotalQuizzesCount
{
    public class GetTotalQuizzesCountQueryHandler
        : IRequestHandler<GetTotalQuizzesCountQuery, RequestResult<Dictionary<Guid, int>>>
    {

        private readonly IGeneralRepository<Quiz> _quizRepo;
        public GetTotalQuizzesCountQueryHandler(IGeneralRepository<Quiz> quizRepo)
            => _quizRepo = quizRepo;

        public async Task<RequestResult<Dictionary<Guid, int>>> Handle(
            GetTotalQuizzesCountQuery request, CancellationToken ct)
        {
            if (!request.DiplomaIds.Any())
                return RequestResult<Dictionary<Guid, int>>.Success(new());

            var counts = await _quizRepo
            .Get(q => request.DiplomaIds.Contains(q.DiplomaId))
            .Published()
            .CountByAsync(q => q.DiplomaId, ct);


            return RequestResult<Dictionary<Guid, int>>.Success(counts);
        }
    }
}

