using ExaminationSystem.Features.Common.FeatureExtensions;
using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.Common.Specifications;
using ExaminationSystem.Features.StudentDashboard.DTOs.Quiz;
using ExaminationSystem.Features.StudentDashboard.Specifications;

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
                return RequestResult<CompletedQuizIdsDto>.Success(
                    new CompletedQuizIdsDto(new()));

            var spec = new StudentAttemptSpecification(request.StudentId, onlyCompleted: true);

            var quizIds = await _attemptRepo
                .GetAll()
                .ApplySpecification(spec)
                .Where(a => request.DiplomaIds.Contains(a.Quiz.DiplomaId))
                .Select(a => a.QuizId)
                .Distinct()
                .ToListAsync(ct);

            return RequestResult<CompletedQuizIdsDto>.Success(
                new CompletedQuizIdsDto(quizIds));
        }
    }
}
