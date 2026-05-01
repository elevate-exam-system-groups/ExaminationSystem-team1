using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Data.Configurations
{
    public class DiplomaConfiguration : IEntityTypeConfiguration<Diploma>
    {
        public void Configure(EntityTypeBuilder<Diploma> builder)
        {
            builder.Property(d => d.Status)
                   .HasConversion<string>();

            builder.Property(d => d.Title)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(d => d.Description)
                   .HasMaxLength(1000);
        }
    }
}
