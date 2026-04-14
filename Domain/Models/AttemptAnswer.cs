using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Models
{
    public class AttemptAnswer : BaseModel
    {
        public Guid QuizAttemptId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid SelectedOptionId { get; set; }
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
