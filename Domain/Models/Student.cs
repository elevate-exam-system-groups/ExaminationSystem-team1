namespace ExaminationSystem.Domain.Models
{
    public class Student : BaseModel
    {
        public User user { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}
