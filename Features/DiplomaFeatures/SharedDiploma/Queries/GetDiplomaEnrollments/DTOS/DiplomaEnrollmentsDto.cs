namespace ExaminationSystem.Features.DiplomaFeatures.SharedDiploma.Queries.GetDiplomaEnrollments.DTOS
{
    public record DiplomaEnrollmentsDto
    {
        public Guid Id { get; init; }
        public Guid DiplomaId { get; init; }
        public string StudentId { get; init; }
        public DateTime EnrolledAt { get; init; }
    }
}
