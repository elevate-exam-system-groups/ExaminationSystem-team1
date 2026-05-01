using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {

            builder.HasKey(q => q.Id);  // Guid primary key
            builder.Property(q => q.Id)
                   .HasDefaultValueSql("NEWID()");  // Auto-generate in DB

            builder.HasMany(q => q.AttemptAnswers)
                   .WithOne(x => x.Question)
                   .HasForeignKey(x => x.QuestionId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(q => q.Quiz)
                   .WithMany(e => e.Questions)
                   .HasForeignKey(q => q.QuizId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
