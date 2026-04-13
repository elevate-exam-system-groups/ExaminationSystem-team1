using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Models
{
    public class QuizAttempt : BaseModel
    {
        public string StudentId { get; set; }
        public int QuizId { get; set; }

        public QuizAttemptStatus Status { get; set; } = QuizAttemptStatus.InProgress;

        public DateTime StartTime { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public decimal? Score { get; set; }
        public bool? IsPassed { get; set; }

        // Navigation properties
        public User Student { get; set; }
        public Quiz Quiz { get; set; }
        public ICollection<AttemptAnswer> UserAnswers { get; set; } = new List<AttemptAnswer>();

    }


    public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            builder.Property(q => q.Status)
                   .HasConversion<string>();

            // Partial Unique Index - the important one
            builder.HasIndex(q => new { q.StudentId, q.QuizId })
                   .IsUnique()
                   .HasFilter("\"Status\" = 'inProgress'");

            // Decimal precision
            builder.Property(q => q.Score)
                   .HasPrecision(5, 2);

            // Nullable columns
            builder.Property(q => q.SubmittedAt)
                   .IsRequired(false);

            builder.Property(q => q.Score)
                   .IsRequired(false);

            builder.Property(q => q.IsPassed)
                   .IsRequired(false);
        }
    }
}
