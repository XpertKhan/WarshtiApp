using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warshti.Entities.WScore;
using WScore.Entities.Identity;

namespace Warshti.Entities.Configurations
{
    public class AnnouncementImageConfiguration : IEntityTypeConfiguration<AnnouncementImage>
    {
        public void Configure(EntityTypeBuilder<AnnouncementImage> builder)
        {
            builder.ToTable("AnnouncementImage", "WScore");
            builder.HasKey(p => p.Id)
                .HasName("PK_AnnouncementImage");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Photo)
                .HasColumnType("VARBINARY(MAX)")
                .IsRequired();

            builder.Property(p => p.AnnouncementId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.Announcement)
                .WithMany(p => p.AnnouncementImages)
                .HasForeignKey(p => p.AnnouncementId)
                .HasConstraintName("FK_AnnouncementImage_Announcement")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
