
namespace ExaminationSystem.Domain.Models
{
    public class Quiz : BaseModel
    {
        public int DiplomaId { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public int PassScore { get; set; }
        public int MaxAttempts { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime PuplishedAt { get; set; }

        // Navigation property
        public Diploma Diploma { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();

    }
}
