using ExaminationSystem.Features.StudentDashboard.Helper;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetCompletedQuizzesCount
{
    public class GetCompletedQuizzesCountQueryHandler
         : IRequestHandler<GetCompletedQuizzesCountQuery, RequestResult<Dictionary<Guid, int>>>
    {

        private readonly IGeneralRepository<Quiz> _quizRepo;
        public GetCompletedQuizzesCountQueryHandler(IGeneralRepository<Quiz> quizRepo)
            => _quizRepo = quizRepo;

        public async Task<RequestResult<Dictionary<Guid, int>>> Handle(
            GetCompletedQuizzesCountQuery request, CancellationToken ct)
        {
            if (!request.CompletedQuizIds.Any())
                return RequestResult<Dictionary<Guid, int>>.Success(new());

            var counts = await _quizRepo
              .Get(q => request.CompletedQuizIds.Contains(q.Id))
              .CountByAsync(q => q.DiplomaId, ct);  
    
            return RequestResult<Dictionary<Guid, int>>.Success(counts);
        }
    }
}

