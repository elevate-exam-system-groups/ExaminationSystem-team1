using ExaminationSystem.Controllers.AdminManagementControllers.ViewStudentAttempts.Mapping;
using ExaminationSystem.Controllers.AdminManagementControllers.ViewStudentAttempts.Pagination;
using ExaminationSystem.Controllers.AdminManagementControllers.ViewStudentAttempts.ViewModels;
using ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.DTOs;
using ExaminationSystem.Features.Common.Paginated.DTOs;

namespace ExaminationSystem.Controllers.AdminManagementControllers.ViewStudentAttempts.Mapping
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
