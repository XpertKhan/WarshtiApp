using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warshti.Entities.WScore;
using WScore.Entities.Identity;

namespace Warshti.Entities.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers", "WScore");
            builder.HasKey(p => p.Id)
                .HasName("PK_Answer");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.AnswerText)
                .HasColumnType("VARCHAR(500)")
                .IsRequired();

            builder.Property(p => p.UserId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.User)
                .WithMany(p => p.Answers)
                .HasForeignKey(p => p.UserId)
                .HasConstraintName("FK_Answer_User")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Question)
                .WithMany(p => p.Answers)
                .HasForeignKey(p => p.QuestionId)
                .HasConstraintName("FK_Answer_Question")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
