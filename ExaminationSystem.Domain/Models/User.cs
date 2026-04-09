

namespace ExaminationSystem.Domain.Models
{
    public class User : BaseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }

        // Navigation properties
        public ICollection<Enrollments> Enrollments { get; set; } = new List<Enrollments>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();

    }
}
