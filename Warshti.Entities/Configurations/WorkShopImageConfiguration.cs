using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warshti.Entities.Maintenance;
using Warshti.Entities.WScore;
using WScore.Entities.Identity;

namespace Warshti.Entities.Configurations
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

            builder.Property(p => p.Photo)
                .HasColumnType("VARBINARY(MAX)")
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
