using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Data.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {

            builder.Property(q => q.DiplomaId)
                   .IsRequired();

            builder.Property(q => q.Status)
                   .HasConversion<string>();

            builder.Property(q => q.PassScore)
                   .HasPrecision(5, 2);

            builder.Property(q => q.Title)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(q => q.PassScore)
                   .HasDefaultValue(60.00);
        }
    }
}
