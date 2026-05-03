using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Models
{
    public class Student : BaseModel
    {
        public User user { get; set; }

        public string UserId { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        //public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }


    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Ignore(o => o.Id);

            builder.HasKey(o => o.UserId);

            builder.HasOne(s => s.user)
          .WithOne()
          .HasForeignKey<Student>(s => s.UserId);

        }
    }



}
