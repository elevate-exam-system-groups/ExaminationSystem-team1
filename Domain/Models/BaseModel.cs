namespace ExaminationSystem.Domain.Models
{
    public class BaseModel
    {
        public int Id { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = default!;
        public string UpdatedBy { get; set; } = default!;
        public bool isDeleted { get; set; }
    }
}
