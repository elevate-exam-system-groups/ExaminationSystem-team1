using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Models.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
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
