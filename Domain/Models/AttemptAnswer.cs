using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Models
{
    public class AttemptAnswer : BaseModel
    {
        public int QuizAttemptId { get; set; }
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }
        public DateTime AnsweredAt { get; set; }
        public bool? IsCorrect { get; set; }
        public QuizAttempt QuizAttempt { get; set; }
        public Question Question { get; set; }
        public Option SelectedOption { get; set; }

    }

    public class AttemptAnswerConfiguration : IEntityTypeConfiguration<AttemptAnswer>
    {
        public void Configure(EntityTypeBuilder<AttemptAnswer> builder)
        {
            builder.HasOne(x => x.QuizAttempt)
                .WithMany(a => a.UserAnswers)
                .HasForeignKey(ua => ua.QuizAttemptId)
                .OnDelete(DeleteBehavior.Cascade); // Keep cascade for Attempt

            builder.HasOne(x => x.Question)
                .WithMany(x => x.AttemptAnswers)
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => new { a.QuizAttemptId, a.QuestionId })
                   .IsUnique();

            builder.HasOne(x => x.SelectedOption)
                   .WithMany(o => o.AttemptAnswers)
                   .HasForeignKey(x => x.SelectedOptionId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
