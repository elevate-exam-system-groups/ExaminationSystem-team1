using ExaminationSystem.Features.StudentDashboard.Helper;

namespace ExaminationSystem.Features.Admin.Queries
{
    public class GetTotalQuizzesQueryHandler : IRequestHandler<GetTotalQuizzesQuery, RequestResult<int>>
    {

        private readonly IGeneralRepository<Quiz> _quizRepo;
        public GetTotalQuizzesQueryHandler(IGeneralRepository<Quiz> quizRepo) 
            => _quizRepo = quizRepo;

        public async Task<RequestResult<int>> Handle
            (GetTotalQuizzesQuery request, CancellationToken ct)
        {
            var count = await _quizRepo
                .GetAll()
                .Published() 
                .CountAsync(ct);

            return RequestResult<int>.Success(count);
        }
    }
}
