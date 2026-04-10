using ExaminationSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Domain.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = default!;
        public Role Role { get; set; }

        // Navigation properties
        //public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();

    }
}
