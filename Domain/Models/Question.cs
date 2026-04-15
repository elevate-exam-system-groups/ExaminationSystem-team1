using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
namespace ExaminationSystem.Domain.Models
{
    public class Question : BaseModel
    {
        [ForeignKey("Quiz")]
        public Guid QuizId { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public int OrderIndex { get; set; } = 1;
        public Quiz? Quiz { get; set; }
        public ICollection<Option> Options { get; set; } = new List<Option>();

        public ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();

    }


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
