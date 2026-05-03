using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Models
{
    public class Admin : BaseModel
    {
        public User user { get; set; }

        public string UserId { get; set; }

    }

    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.Ignore(o => o.Id);

            builder.HasKey(o => o.UserId);

            builder.HasOne(a => a.user)
          .WithOne()
          .HasForeignKey<Admin>(a => a.UserId);


        }
    }
}
