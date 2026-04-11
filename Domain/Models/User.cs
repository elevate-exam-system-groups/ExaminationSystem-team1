using ExaminationSystem.ExaminationSystem.Domain.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Domain.Models
{
    public class User : IdentityUser//BaseModel
    {
        public string FullName { get; set; }
        //public string Email { get; set; }
        //public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public AccountStatus accountStatus { get; set; }

        // Navigation properties
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();

        // Additional properties expected by PasswordReset feature
        public DateTime? ForgotPasswordLockoutEnd { get; set; }
        public int ForgotPasswordAttempts { get; set; }
        public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
    }
}
