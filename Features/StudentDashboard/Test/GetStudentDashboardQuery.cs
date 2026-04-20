using ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard.Test
{
    public record GetStudentDashboardQuery(string StudentId)
    : IRequest<RequestResult<StudentDashboardResponse>>;
}
