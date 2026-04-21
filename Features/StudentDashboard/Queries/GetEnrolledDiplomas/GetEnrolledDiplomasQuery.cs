using ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard.Queries
{
    public record GetEnrolledDiplomasQuery(string StudentId)
        : IRequest<RequestResult<EnrolledDiplomasListDto>>;
}

