using ExaminationSystem.Controllers.AdminDashboard.ViewModels;
using ExaminationSystem.Features.AdminDashboard.DTOs;

namespace ExaminationSystem.Controllers.AdminDashboard.Mapping
{
    public static class AdminDashboardMapper
    {
        public static AdminStatsVm FromDto(AdminStatsResponseDto dto)
        {
            return new AdminStatsVm(
                dto.TotalUsers,
                dto.ActiveUsersToday,
                dto.TotalQuizzes,
                dto.TotalAttempts,
                dto.AvgPassRate
            );
        }
    }
}
