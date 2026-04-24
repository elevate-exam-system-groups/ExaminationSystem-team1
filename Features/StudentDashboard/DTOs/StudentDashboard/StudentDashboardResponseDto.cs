using ExaminationSystem.Features.StudentDashboard.DTOs.Attempt;
using ExaminationSystem.Features.StudentDashboard.DTOs.Diploma;
using ExaminationSystem.Features.StudentDashboard.DTOs.OverallStats;

namespace ExaminationSystem.Features.StudentDashboard.DTOs.StudentDashboard
{

    public record StudentDashboardResponseDto(
     List<EnrolledDiplomaDto> EnrolledDiplomas,
     List<RecentAttemptDto> RecentQuizAttempts,
     OverallStatsDto OverallStats
    );
}
