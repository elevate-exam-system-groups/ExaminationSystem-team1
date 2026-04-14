using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Domain.Models
{
    public class Enrollment : BaseModel
    {
        public string StudentId { get; set; }
        public Guid DiplomaId { get; set; }
        public DateTime EnrollmentDate { get; set; }


        [ForeignKey(nameof(StudentId))]
        public User Student { get; set; }

        [ForeignKey(nameof(DiplomaId))]
        public Diploma Diploma { get; set; }
    }


    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasIndex(e => new { e.StudentId, e.DiplomaId })
                   .IsUnique();
        }
    }
}
