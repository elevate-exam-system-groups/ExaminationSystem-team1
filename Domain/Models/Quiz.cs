using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Domain.Models
{
    public class Quiz : BaseModel
    {
        [ForeignKey("Diploma")]
        public int DiplomaId { get; set; }
        public Diploma Diploma { get; set; }
        public string Title { get; set; } = string.Empty;

        public string? Instructions { get; set; }
        public decimal PassScore { get; set; }
        public int? MaxAttempts { get; set; }
        public int DurationInMinutes { get; set; }
        public QuizStatus Status { get; set; } = QuizStatus.Draft;

        public DateTime? PublishedAt { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();

    }

    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
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
