namespace ExaminationSystem.Features.StudentDashboard.DTOs
{

    public record StudentDashboardResponseDto(
     List<EnrolledDiplomaDto> EnrolledDiplomas,
     List<RecentAttemptDto> RecentQuizAttempts,
     OverallStatsDto OverallStats
    );
}
