using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WScore.Data.Entities.Maintenance;
using WScore.Data.Entities.WScore;
using WScore.Entities.Identity;

namespace WScore.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order", "Maintenance");
            builder.HasKey(p => p.Id)
                .HasName("PK_Order");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.ServiceId)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.EstimatedPrice)
                .HasColumnType("NUMERIC(10,2)")
                .IsRequired();

            builder.HasOne(p => p.Service)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.ServiceId)
                .HasConstraintName("FK_Order_Service")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.WorkshopId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.Workshop)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.WorkshopId)
                .HasConstraintName("FK_Order_WorkShop")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.CreationDate)
                .HasColumnType("DATE")
                .IsRequired();

            builder.Property(p => p.ExpectedCompletionDate)
                .HasColumnType("DATE")
                .IsRequired();

            builder.Property(p => p.CompletionDate)
                .HasColumnType("DATE")
                .IsRequired(false);

            builder.Property(p => p.OrderNumber)
                .HasColumnType("VARCHAR(25)")
                .IsRequired(false);

            builder.Property(p => p.OrderStatusId)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.OrderProgress)
                .HasColumnType("INT")
                .IsRequired();
        }
    }
}
