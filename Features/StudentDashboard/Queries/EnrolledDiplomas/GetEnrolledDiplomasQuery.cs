using ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard.Queries.EnrolledDiplomas
{
    public record GetEnrolledDiplomasQuery(string StudentId)
        : IRequest<RequestResult<EnrolledDiplomasResponseDto>>;
}

