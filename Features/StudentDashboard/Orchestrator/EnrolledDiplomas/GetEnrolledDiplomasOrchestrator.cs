using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.StudentDashboard.DTOs.Diploma;

namespace ExaminationSystem.Features.StudentDashboard.Orchestrator.EnrolledDiplomas
{
    public record GetEnrolledDiplomasOrchestrator(string StudentId)
        : IRequest<RequestResult<EnrolledDiplomasListDto>>;
}

