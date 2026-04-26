using ExaminationSystem.Controllers.Monitoring_Analytics.ViewStudentAttempts.ViewModels;
using ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.DTOs;
using ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Paginated;

namespace ExaminationSystem.Controllers.Monitoring_Analytics.ViewStudentAttempts.Mapping
{
    public static class AttemptMapper
    {
        public static PaginatedResponseVm<AttemptVm> ToViewModel(
            this PaginatedResponseDto<AttemptDto> dto)
        {
            return dto.ToViewModel(ToViewModel);
        }

        public static AttemptVm ToViewModel(this AttemptDto dto)
        {
            return new AttemptVm(
                dto.AttemptId,
                dto.StudentId,
                dto.StudentName,
                dto.StudentEmail,
                dto.QuizId,
                dto.QuizTitle,
                dto.Status,
                dto.Score,
                dto.IsPassed,
                dto.StartTime,
                dto.SubmittedAt
            );
        }

        public static AttemptDetailVm ToViewModel(this AttemptDetailDto dto)
        {
            return new AttemptDetailVm(
                dto.AttemptId,
                dto.StudentId,
                dto.StudentName,
                dto.StudentEmail,
                dto.QuizId,
                dto.QuizTitle,
                dto.Status,
                dto.Score,
                dto.IsPassed,
                dto.StartTime,
                dto.SubmittedAt,
                dto.DurationInMinutes,
                dto.Answers.Select(ToViewModel).ToList()
            );
        }

        public static AttemptAnswerVm ToViewModel(this AttemptAnswerDto dto)
        {
            return new AttemptAnswerVm(
                dto.QuestionId,
                dto.QuestionText,
                dto.OrderIndex,
                dto.SelectedOptionId,
                dto.SelectedOptionText,
                dto.IsCorrect,
                dto.CorrectOptionId,
                dto.CorrectOptionText,
                dto.Explanation
            );
        }
    }
}
