using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Domain.Models
{
    public class Quiz : BaseModel
    {
        [ForeignKey("Diploma")]
        public Guid DiplomaId { get; set; }
        public Diploma Diploma { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Instructions { get; set; }
        public decimal PassScore { get; set; }
        public int? MaxAttempts { get; set; }
        public int DurationInMinutes { get; set; }
        public QuizStatus Status { get; set; } = QuizStatus.Draft;

        public DateTime? PublishedAt { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();

    }

   
}
