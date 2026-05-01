using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Data.Configurations
{
    public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            builder.Property(q => q.Status)
                   .HasConversion<string>();

            // Partial Unique Index - the important one
            builder.HasIndex(q => new { q.StudentId, q.QuizId })
                   .IsUnique()
                   .HasFilter("[Status] = 'InProgress'");

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
