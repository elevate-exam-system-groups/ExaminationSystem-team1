using ExaminationSystem.ExaminationSystem.Domain.Models.Enums;

namespace ExaminationSystem.Domain.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public Role Role { get; set; }
        public AccountStatus accountStatus { get; set; }

        // Navigation properties
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();

       
        //  Manage => Reset Password
        public string? PasswordResetTokenHash { get; set; } 
        public DateTime? PasswordResetTokenExpiry { get; set; }
        public bool IsPasswordResetTokenUsed { get; set; }

        public DateTime? ForgotPasswordLockoutEnd { get; set; }
        public int ForgotPasswordAttempts { get; set; }
        public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();

    }
}
