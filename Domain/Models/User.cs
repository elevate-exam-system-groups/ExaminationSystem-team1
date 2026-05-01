using ExaminationSystem.ExaminationSystem.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime? LastActivityAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public AccountStatus accountStatus { get; set; } = AccountStatus.Active;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();

        public DateTime? ForgotPasswordLockoutEnd { get; set; }
        public int ForgotPasswordAttempts { get; set; }
        public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<UserOTP> UserOTPs { get; set; } = new List<UserOTP>();

    }
}
