using ExaminationSystem.Controllers.StudentController.ViewModels;
using ExaminationSystem.Features.StudentDashboard.DTOs.Attempt;
using ExaminationSystem.Features.StudentDashboard.DTOs.Diploma;
using ExaminationSystem.Features.StudentDashboard.DTOs.OverallStats;
using ExaminationSystem.Features.StudentDashboard.DTOs.StudentDashboard;

namespace ExaminationSystem.Controllers.StudentController.Mapping
{
    public static class StudentDashboardMapper
    {
        public static StudentDashboardResponseVm ToViewModel(this StudentDashboardResponseDto dto)
        {
            return new StudentDashboardResponseVm(
                EnrolledDiplomas: dto.EnrolledDiplomas.Select(d => d.ToViewModel()).ToList(),
                RecentQuizAttempts: dto.RecentQuizAttempts.Select(a => a.ToViewModel()).ToList(),
                OverallStats: dto.OverallStats.ToViewModel()
            );
        }

        private static EnrolledDiplomaVm ToViewModel(this EnrolledDiplomaDto dto)
        {
            return new EnrolledDiplomaVm(
                dto.DiplomaId,
                dto.Title,
                dto.Description,
                dto.TotalQuizzes,
                dto.CompletedQuizzes,
                dto.ProgressPercentage
            );
        }

        private static RecentAttemptVm ToViewModel(this RecentAttemptDto dto)
        {
            return new RecentAttemptVm(
                dto.AttemptId,
                dto.QuizId,
                dto.QuizTitle,
                dto.DiplomaTitle,
                dto.Status,
                dto.Score,
                dto.IsPassed,
                dto.SubmittedAt
            );
        }

        private static OverallStatsVm ToViewModel(this OverallStatsDto dto)
        {
            return new OverallStatsVm(
                dto.TotalQuizzesTaken,
                dto.AvgScore,
                dto.PassRate
            );
        }
    }
}
