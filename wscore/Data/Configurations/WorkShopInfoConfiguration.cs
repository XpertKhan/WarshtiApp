using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WScore.Data.Entities.Car;
using WScore.Data.Entities.Maintenance;
using WScore.Entities.Identity;

namespace WScore.Configurations
{
    public class WorkShopInfoConfiguration : IEntityTypeConfiguration<WorkShopInfo>
    {
        public void Configure(EntityTypeBuilder<WorkShopInfo> builder)
        {
            builder.ToTable("WorkShopInfo", "Maintenance");

            builder.HasKey(p => p.Id)
                .HasName("PK_WorkShopInfo");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.CommercialRegister)
                .HasColumnType("VARCHAR(50)")
                .IsRequired(false);

            builder.Property(p => p.Department)
                .HasColumnType("VARCHAR(50)")
                .IsRequired(false);

            builder.Property(p => p.ElectonicPaymentAccount)
                .HasColumnType("VARCHAR(50)")
                .IsRequired(false);

            builder.Property(p => p.Facility)
                .HasColumnType("VARCHAR(50)")
                .IsRequired(false);

            builder.Property(p => p.ImagePath)
                .HasColumnType("VARCHAR(250)")
                .IsRequired(false);

            builder.Property(p => p.Latitude)
                .HasColumnType("NUMERIC(18,10)")
                .IsRequired(false);

            builder.Property(p => p.Longitude)
                .HasColumnType("NUMERIC(18,10)")
                .IsRequired(false);

            builder.Property(p => p.Sponsor)
                .HasColumnType("VARCHAR(150)")
                .IsRequired(false);

            builder.Property(p => p.WorkShopId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(b => b.Workshop)
            .WithOne(i => i.WorkShopInfo)
            .HasForeignKey<WorkShopInfo>(b => b.WorkShopId);
        }
    }
}
