namespace ExaminationSystem.Domain.Models
{
    public class BaseModel<TKey>
    {
        public TKey Id { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = default!;
        public string UpdatedBy { get; set; } = default!;
        public bool isDeleted { get; set; }
    }
}
