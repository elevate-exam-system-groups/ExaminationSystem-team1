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
                        .Select(ua => new 
                        {
                            ua.QuestionId,
                            ua.Question.Text,
                            ua.Question.OrderIndex,
                            ua.SelectedOptionId,
                            SelectedOptionText = ua.SelectedOption.Text ?? "N/A",
                            ua.IsCorrect,
                            ua.Question.Explanation,
                            CorrectOption = ua.Question.Options
                                .Where(o => o.IsCorrect && !o.IsDeleted)
                                .Select(o => new { o.Id, o.Text })
                                .FirstOrDefault()
                        })
                        .Select(x => new AttemptAnswerDto
                        (
                            x.QuestionId,
                            x.Text,
                            x.OrderIndex,
                            x.SelectedOptionId,
                            x.SelectedOptionText,
                            x.IsCorrect,
                            x.CorrectOption != null ? x.CorrectOption.Id : Guid.Empty,
                            x.CorrectOption != null ? x.CorrectOption.Text : "N/A",
                            x.Explanation
                        )).ToList()

                )).FirstOrDefaultAsync(ct);

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