using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warshti.Entities.Entities.Maintenance;
using Warshti.Entities.WScore;
using WScore.Entities.Identity;

namespace Warshti.Entities.Configurations
{
    public class ServiceImageConfiguration : IEntityTypeConfiguration<ServiceImage>
    {
        public void Configure(EntityTypeBuilder<ServiceImage> builder)
        {
            builder.ToTable("ServiceImage", "WScore");
            builder.HasKey(p => p.Id)
                .HasName("PK_ServiceImage");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.ServiceId)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.Photo)
                .HasColumnType("VARBINARY(MAX)")
                .IsRequired();

            builder.HasOne(p => p.Service)
                .WithMany(p => p.ServiceImages)
                .HasForeignKey(p => p.ServiceId)
                .HasConstraintName("FK_ServiceImage_Service")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
