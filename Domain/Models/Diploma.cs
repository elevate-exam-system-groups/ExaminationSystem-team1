using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Models
{
    public class Diploma : BaseModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DiplomaStatus Status { get; set; } = DiplomaStatus.Draft;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    }

    public class DiplomaConfiguration : IEntityTypeConfiguration<Diploma>
    {
        public void Configure(EntityTypeBuilder<Diploma> builder)
        {
            builder.Property(d => d.Status)
                   .HasConversion<string>();

            builder.Property(d => d.Title)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(d => d.Description)
                   .HasMaxLength(1000);
        }
    }
}
