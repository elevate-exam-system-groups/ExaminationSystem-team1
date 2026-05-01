using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Data.Configurations
{

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.Property(u => u.Role)
            //       .HasConversion<string>();

            builder.Property(u => u.accountStatus)
                   .HasConversion<string>();

            builder.HasIndex(u => u.LastActivityAt)
                   .HasFilter("[LastActivityAt] IS NOT NULL");

        }
    }
}
