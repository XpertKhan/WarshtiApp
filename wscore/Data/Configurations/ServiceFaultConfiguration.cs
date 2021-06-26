using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WScore.Data.Entities.Car;
using WScore.Data.Entities.Maintenance;
using WScore.Entities.Identity;

namespace WScore.Configurations
{
    public class ServiceFaultConfiguration : IEntityTypeConfiguration<ServiceFault>
    {
        public void Configure(EntityTypeBuilder<ServiceFault> builder)
        {
            builder.ToTable("ServiceFault", "Maintenance");

            builder.HasKey(p => p.Id)
                .HasName("PK_ServiceFault");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.FaultId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.Fault)
                .WithMany(p => p.ServiceFaults)
                .HasForeignKey(p => p.FaultId)
                .HasConstraintName("FK_ServiceFault_Fault")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.ServiceId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.Service)
                .WithMany(p => p.ServiceFaults)
                .HasForeignKey(p => p.ServiceId)
                .HasConstraintName("FK_ServiceFault_Service")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
