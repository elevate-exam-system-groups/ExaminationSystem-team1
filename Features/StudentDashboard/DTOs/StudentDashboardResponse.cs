namespace ExaminationSystem.Features.StudentDashboard.DTOs
{
    public record StudentDashboardResponse(
     List<EnrolledDiplomaDto> EnrolledDiplomas,
     List<RecentAttemptDto> RecentQuizAttempts,
     OverallStatsDto OverallStats
    );
}
