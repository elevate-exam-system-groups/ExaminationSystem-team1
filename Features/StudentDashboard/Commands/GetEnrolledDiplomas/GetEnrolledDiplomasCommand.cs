using ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard
{
    namespace ExaminationSystem.Features.StudentDashboard.Queries
    {
        public record GetEnrolledDiplomasCommand(string StudentId)
            : IRequest<RequestResult<List<EnrolledDiplomaDto>>>;
    }
}
