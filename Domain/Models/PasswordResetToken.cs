using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

    public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
    {
        public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
        {
            // relation => User و PasswordResetToken
            builder.HasOne(e => e.User)
                      .WithMany(u => u.PasswordResetTokens)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
