using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WScore.Data.Entities.Car;
using WScore.Data.Entities.Identity;
using WScore.Data.Entities.WScore;
using WScore.Entities.Identity;

namespace WScore.Configurations
{
    public class UserSettingConfiguration : IEntityTypeConfiguration<UserSetting>
    {
        public void Configure(EntityTypeBuilder<UserSetting> builder)
        {
            builder.ToTable("UserSetting", "Identity");

            builder.HasKey(p => p.Id)
                .HasName("PK_UserSetting");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.AcceptRequest)
                .HasColumnType("BIT")
                .IsRequired();

            builder.Property(p => p.DeclineRequest)
                .HasColumnType("BIT")
                .IsRequired();

            builder.Property(p => p.EmailNotification)
                .HasColumnType("BIT")
                .IsRequired();

            builder.Property(p => p.LanguageId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.Language)
                .WithMany(p => p.UserSettings)
                .HasForeignKey(p => p.LanguageId)
                .HasConstraintName("FK_UserSetting_Language")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.User)
            .WithOne(i => i.UserSetting)
            .HasForeignKey<UserSetting>(b => b.UserId);
        }
    }
}
