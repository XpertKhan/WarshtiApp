using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warshti.Entities.WScore;
using WScore.Entities.Identity;

namespace Warshti.Entities.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions", "WScore");
            builder.HasKey(p => p.Id)
                .HasName("PK_Questions");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Description)
                .HasColumnType("VARCHAR(500)")
                .IsRequired();

            builder.Property(p => p.Detail)
                .HasColumnType("VARCHAR(2000)")
                .IsRequired();

            builder.Property(p => p.Title)
                .HasColumnType("VARCHAR(250)")
                .IsRequired();

            builder.Property(p => p.UserId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.User)
                .WithMany(p => p.Questions)
                .HasForeignKey(p => p.UserId)
                .HasConstraintName("FK_Question_User")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
