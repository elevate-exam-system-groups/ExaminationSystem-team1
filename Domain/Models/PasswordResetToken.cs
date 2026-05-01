namespace ExaminationSystem.Domain.Models
{
    public class PasswordResetToken : BaseModel
    {
        public string UserId { get; set; }
        public string TokenHash { get; set; }
        public DateTime ExpiryAt { get; set; }
        public bool IsUsed { get; set; } 


        // Navigation
        public User User { get; set; }
    }
}
