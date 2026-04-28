using ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.DTOs;
using ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.Specifications;
using ExaminationSystem.Features.Common.Specifications;

namespace ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.Queries.GetAttemptDetail
{
    public class GetAttemptDetailQueryHandler
        : IRequestHandler<GetAttemptDetailQuery, RequestResult<AttemptDetailDto>>
    {

        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        public GetAttemptDetailQueryHandler(IGeneralRepository<QuizAttempt> attemptRepo)
        {
            _attemptRepo = attemptRepo;
        }

        public async Task<RequestResult<AttemptDetailDto>> Handle(
            GetAttemptDetailQuery request, CancellationToken ct)
        {

            var spec = new AttemptDetailSpecification(request.AttemptId);

            var attempt = await _attemptRepo
                .GetAll()
                .ApplySpecification(spec)
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
                                .Where(o => o.IsCorrect && !o.IsDeleted)
                                .Select(o => o.Id)
                                .FirstOrDefault(),
                            ua.Question.Options
                                .Where(o => o.IsCorrect && !o.IsDeleted)
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