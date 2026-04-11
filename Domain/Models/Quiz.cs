using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Domain.Models
{
    public class Quiz : BaseModel
    {
        [ForeignKey("Diploma")]
        public int DiplomaId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Instructions { get; set; }= string.Empty;
        public int PassScore { get; set; }
        public int MaxAttempts { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public DateTime PublishedAt { get; set; }

        // Navigation property
        public Diploma? Diploma { get; set; }
        public int DiplomaId { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public int PassScore { get; set; }
        public int MaxAttempts { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }


        public ICollection<Question> Questions { get; set; } = new List<Question>();

    }
}
