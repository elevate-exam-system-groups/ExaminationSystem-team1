namespace ExaminationSystem.Domain.Models
{
    public class Diploma : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public ICollection<Enrollments> Enrollments { get; set; } = new List<Enrollments>();

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();


    }
}
