using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Data.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {

            builder.Ignore(o => o.Id);

            builder.HasKey(o => o.UserId);

        }
    }
}
