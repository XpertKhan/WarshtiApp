using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warshti.Entities.Car;
using Warshti.Entities.WScore;
using WScore.Entities.Identity;

namespace Warshti.Entities.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification", "WScore");

            builder.HasKey(p => p.Id)
                .HasName("PK_Notification");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Message)
                .HasColumnType("VARCHAR(500)")
                .IsRequired();

            builder.Property(p => p.UserId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.User)
                .WithMany(p => p.Notifications)
                .HasForeignKey(p => p.UserId)
                .HasConstraintName("FK_Notification_User")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.NotificationTime)
                .HasColumnType("DATETIME")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
