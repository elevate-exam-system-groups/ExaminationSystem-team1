using ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.DTOs;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Queries.GetAttemptDetail
{
    public class GetAttemptDetailQueryHandler
       : IRequestHandler<GetAttemptDetailQuery, RequestResult<AttemptDetailDto>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetAttemptDetailQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
            => _attemptRepo = attemptRepo;


        public async Task<RequestResult<AttemptDetailDto>> Handle(
           GetAttemptDetailQuery request, CancellationToken ct)
        {

            var attempt = await _attemptRepo
                .GetAll()
                .Where(a => a.Id == request.AttemptId && !a.IsDeleted)
                .Select(a => new AttemptDetailDto(
                    a.Id,
                    a.StudentId,
                    a.Student.FullName,
                    a.Student.Email,
                    a.QuizId,
                    a.Quiz.Title,
                    a.Status.ToString(),
                    a.Score,
                    a.IsPassed,
                    a.StartTime,
                    a.SubmittedAt,
                    a.Quiz.DurationInMinutes,
                    a.UserAnswers
                        .OrderBy(ua => ua.Question.OrderIndex)
                        .Select(ua => new AttemptAnswerDto(
                            ua.QuestionId,
                            ua.Question.Text,
                            ua.Question.OrderIndex,
                            ua.SelectedOptionId,
                            ua.SelectedOption.Text ?? "N/A",
                            ua.IsCorrect,
                            ua.Question.Options
                                .Where(o => o.IsCorrect)
                                .Select(o => o.Id)
                                .FirstOrDefault(),
                            ua.Question.Options
                                .Where(o => o.IsCorrect)
                                .Select(o => o.Text)
                                .FirstOrDefault() ?? "N/A",
                            ua.Question.Explanation
                        )).ToList()
                ))
                .FirstOrDefaultAsync(ct);

            if (attempt == null)
            {
                return RequestResult<AttemptDetailDto>.Failure(
                    "Attempt not found",
                    RequestErrorCode.NotFound);
            }

            return RequestResult<AttemptDetailDto>.Success(attempt);
        }
    }
}