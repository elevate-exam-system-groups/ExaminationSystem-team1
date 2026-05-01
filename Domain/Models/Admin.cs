namespace ExaminationSystem.Domain.Models
{
    public class Admin : BaseModel
    {
        public User user { get; set; }

        public string UserId { get; set; }

    } 
}
