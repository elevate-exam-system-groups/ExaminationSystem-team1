using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.StudentDashboard.DTOs.Quiz;
using ExaminationSystem.Features.StudentDashboard.Helper;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetTotalQuizzesCount
{
    public class GetTotalQuizzesCountQueryHandler
        : IRequestHandler<GetTotalQuizzesCountQuery, RequestResult<TotalQuizzesCountDto>>
    {

        private readonly IGeneralRepository<Quiz> _quizRepo;
        public GetTotalQuizzesCountQueryHandler(IGeneralRepository<Quiz> quizRepo)
            => _quizRepo = quizRepo;

        public async Task<RequestResult<TotalQuizzesCountDto>> Handle(
            GetTotalQuizzesCountQuery request, CancellationToken ct)
        {
            if (!request.DiplomaIds.Any())       //=======================
                return RequestResult<TotalQuizzesCountDto>.Success(
                    new TotalQuizzesCountDto(new()));

            var counts = await _quizRepo
            .Get(q => request.DiplomaIds.Contains(q.DiplomaId))
            .Published()
            .CountByAsync(q => q.DiplomaId, ct);


            return RequestResult<TotalQuizzesCountDto>.Success(
                new TotalQuizzesCountDto(counts));
        }
    }
}

