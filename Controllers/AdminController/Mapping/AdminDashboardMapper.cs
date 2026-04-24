using ExaminationSystem.Controllers.AdminController.ViewModels;
using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Controllers.AdminController.Mapping
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
