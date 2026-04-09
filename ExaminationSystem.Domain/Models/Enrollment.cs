namespace ExaminationSystem.Domain.Models
{
    public class Enrollment : BaseModel<int>
    {
        public int StudentId { get; set; }
        public int DiplomaId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; }

        // Navigation properties
        public User Student { get; set; }
        public Diploma Diploma { get; set; }
    }
}
