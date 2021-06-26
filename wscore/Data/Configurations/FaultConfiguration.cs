using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WScore.Data.Entities.Car;
using WScore.Data.Entities.Maintenance;
using WScore.Entities.Identity;

namespace WScore.Configurations
{
    public class FaultConfiguration : IEntityTypeConfiguration<Fault>
    {
        public void Configure(EntityTypeBuilder<Fault> builder)
        {
            builder.ToTable("Fault", "Maintenance");

            builder.HasKey(p => p.Id)
                .HasName("PK_Fault");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .HasColumnType("VARCHAR(250)")
                .IsRequired();

        }
    }
}
