using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Queries
{
    public class GetTotalQuizzesQueryHandler 
        : IRequestHandler<GetTotalQuizzesQuery, RequestResult<TotalQuizzesDto>>
    {

        private readonly IGeneralRepository<Quiz> _quizRepo;
        public GetTotalQuizzesQueryHandler(IGeneralRepository<Quiz> quizRepo) 
            => _quizRepo = quizRepo;

        public async Task<RequestResult<TotalQuizzesDto>> Handle
            (GetTotalQuizzesQuery request, CancellationToken ct)
        {
            var count = await _quizRepo
                .Get(q => q.Status == QuizStatus.Published && !q.isDeleted)
                .CountAsync(ct);

            return RequestResult<TotalQuizzesDto>.Success(new TotalQuizzesDto(count));
        }
    }
}
