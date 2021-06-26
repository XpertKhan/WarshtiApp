using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warshti.Entities.WScore;
using WScore.Entities.Identity;

namespace Warshti.Entities.Configurations
{
    public class QuestionImageConfiguration : IEntityTypeConfiguration<QuestionImage>
    {
        public void Configure(EntityTypeBuilder<QuestionImage> builder)
        {
            builder.ToTable("QuestionImage", "WScore");
            builder.HasKey(p => p.Id)
                .HasName("PK_QuestionImage");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Photo)
                .HasColumnType("VARBINARY(MAX)")
                .IsRequired();

            builder.Property(p => p.Question)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.Question)
                .WithMany(p => p.QuestionImages)
                .HasForeignKey(p => p.QuestionId)
                .HasConstraintName("FK_QuestionImage_Announcement")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
