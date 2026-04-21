using ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard
{
    public record GetStudentDashboardOrchestrator(string StudentId)
    : IRequest<RequestResult<StudentDashboardResponse>>;
}
