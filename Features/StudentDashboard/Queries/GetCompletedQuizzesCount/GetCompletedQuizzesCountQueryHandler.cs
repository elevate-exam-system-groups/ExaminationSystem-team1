using ExaminationSystem.Features.StudentDashboard.DTOs.Quiz;
using ExaminationSystem.Features.StudentDashboard.Helper;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetCompletedQuizzesCount
{
    public class GetCompletedQuizzesCountQueryHandler
         : IRequestHandler<GetCompletedQuizzesCountQuery, RequestResult<CompletedQuizzesCountDto>>
    {

        private readonly IGeneralRepository<Quiz> _quizRepo;
        public GetCompletedQuizzesCountQueryHandler(IGeneralRepository<Quiz> quizRepo)
            => _quizRepo = quizRepo;

        public async Task<RequestResult<CompletedQuizzesCountDto>> Handle(
            GetCompletedQuizzesCountQuery request, CancellationToken ct)
        {

            if (!request.CompletedQuizIds.Any())   //=======================
                return RequestResult<CompletedQuizzesCountDto>.Success(
                    new CompletedQuizzesCountDto(new()));

            var counts = await _quizRepo
              .Get(q => request.CompletedQuizIds.Contains(q.Id))
              .CountByAsync(q => q.DiplomaId, ct);  
    
            return RequestResult<CompletedQuizzesCountDto>.Success(
                new CompletedQuizzesCountDto(counts));
        }
    }
}

