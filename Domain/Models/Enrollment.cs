using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Domain.Models
{
    public class Enrollment : BaseModel
    {
        public string StudentId { get; set; }
        public int DiplomaId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; }

        // Navigation properties
        [ForeignKey("StudentId")]
        public User Student { get; set; }
        public Diploma Diploma { get; set; }
    }
}
