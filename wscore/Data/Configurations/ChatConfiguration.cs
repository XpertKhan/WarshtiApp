using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WScore.Data.Entities.WScore;
using WScore.Entities.Identity;

namespace WScore.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.ToTable("Chat", "WScore");
            builder.HasKey(p => p.Id)
                .HasName("PK_Chat");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Message)
                .HasColumnType("VARCHAR(2000)")
                .IsRequired();

            builder.Property(p => p.MessageTime)
                .HasColumnType("DATETIME")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.SenderId)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.ReceiverId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.Receiver)
                .WithMany(p => p.ReceiverChats)
                .HasForeignKey(p => p.ReceiverId)
                .HasConstraintName("FK_Chat_Receiver")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Sender)
                .WithMany(p => p.SenderChats)
                .HasForeignKey(p => p.SenderId)
                .HasConstraintName("FK_Chat_Sender")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
