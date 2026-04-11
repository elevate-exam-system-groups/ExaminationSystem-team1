namespace ExaminationSystem.Domain.Models
{
    public class PasswordResetToken : BaseModel
    {
        public string UserId { get; set; } // مين طلب ال token
        public string TokenHash { get; set; } //ايه هو ال token
        public DateTime ExpiryAt { get; set; } //ينتهي امتي؟
        public bool IsUsed { get; set; } // استخدم ولا لا


        // Navigation
        public User User { get; set; }
    }
}
