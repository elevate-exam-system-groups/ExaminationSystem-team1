namespace ExaminationSystem.Controllers.StudentController.ViewModels
{
    public record StudentDashboardResponseVm(
    List<EnrolledDiplomaVm> EnrolledDiplomas,
    List<RecentAttemptVm> RecentQuizAttempts,
    OverallStatsVm OverallStats
);
}
