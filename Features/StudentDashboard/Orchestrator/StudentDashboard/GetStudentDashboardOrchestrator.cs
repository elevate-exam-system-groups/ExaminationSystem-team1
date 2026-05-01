using ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard.Orchestrator.StudentDashboard
{
    public record GetStudentDashboardOrchestrator(string StudentId)
    : IRequest<RequestResult<StudentDashboardResponseDto>>;
}
