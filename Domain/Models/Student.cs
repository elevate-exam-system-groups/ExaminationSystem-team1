using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Models
{
    public class Student : BaseModel
    {
        public User user { get; set; }

        public string UserId { get; set; }
    }


    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Ignore(o => o.Id);

            builder.HasKey(o => o.UserId);

            builder.HasOne(o => o.user)
                   .WithOne()
                   .HasForeignKey<Student>(o => o.UserId);
        }
    }



}
