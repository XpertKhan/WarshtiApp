using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warshti.Entities.Car;
using Warshti.Entities.Maintenance;
using WScore.Entities.Identity;

namespace Warshti.Entities.Configurations
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
