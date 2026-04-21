using ExaminationSystem.Features.StudentDashboard.DTOs;
using ExaminationSystem.Features.StudentDashboard.Helper;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetCompletedQuizIds
{
    public class GetCompletedQuizIdsQueryHandler
     : IRequestHandler<GetCompletedQuizIdsQuery, RequestResult<CompletedQuizIdsDto>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetCompletedQuizIdsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;

        public async Task<RequestResult<CompletedQuizIdsDto>> Handle(
            GetCompletedQuizIdsQuery request, CancellationToken ct)
        {
            if (!request.DiplomaIds.Any())
                return RequestResult<CompletedQuizIdsDto>.Success(new CompletedQuizIdsDto(new()));

            var quizIds = await _attemptRepo
                .Get(a => a.StudentId == request.StudentId
                       && request.DiplomaIds.Contains(a.Quiz.DiplomaId))
                .Completed()
                .Select(a => a.QuizId)
                .Distinct()
                .ToListAsync(ct);

            return RequestResult<CompletedQuizIdsDto>.Success(new CompletedQuizIdsDto(quizIds));
        }
    }
}

