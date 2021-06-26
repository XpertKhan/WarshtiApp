using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WScore.Data.Entities.Maintenance;
using WScore.Data.Entities.WScore;
using WScore.Entities.Identity;

namespace WScore.Configurations
{
    public class WorkShopImageConfiguration : IEntityTypeConfiguration<WorkShopImage>
    {
        public void Configure(EntityTypeBuilder<WorkShopImage> builder)
        {
            builder.ToTable("WorkShopImage", "Maintenance");
            builder.HasKey(p => p.Id)
                .HasName("PK_WorkShopImage");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.ImagePath)
                .HasColumnType("VARCHAR(1000)")
                .IsRequired();

            builder.Property(p => p.WorkShopId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.WorkShop)
                .WithMany(p => p.WorkShopImages)
                .HasForeignKey(p => p.WorkShopId)
                .HasConstraintName("FK_WorkShopImage_WorkShop")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
