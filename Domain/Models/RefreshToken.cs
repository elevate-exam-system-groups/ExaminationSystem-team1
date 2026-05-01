using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Domain.Models
{
    public class RefreshToken : BaseModel
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        public string JwtId { get; set; } = string.Empty;
        
        public bool IsUsed { get; set; }
        
        public bool IsRevoked { get; set; }
        
        public DateTime AddedDate { get; set; }
        
        public DateTime ExpiryDate { get; set; }


        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
    }
}
