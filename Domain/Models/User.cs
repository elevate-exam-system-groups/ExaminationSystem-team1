namespace ExaminationSystem.Domain.Models
{
    public class User : IdentityUser
    {

        //  Manage => Reset Password
        public string? PasswordResetTokenHash { get; set; } // to store Hash for token 
        public DateTime? PasswordResetTokenExpiry { get; set; }
        public bool IsPasswordResetTokenUsed { get; set; }


        //  تتبع محاولات الفشل للـ Forgot Password
        public int ForgotPasswordAttempts { get; set; }
        public DateTime? ForgotPasswordLockoutEnd { get; set; }


        public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; }
    }
}
