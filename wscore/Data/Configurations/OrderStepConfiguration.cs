using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WScore.Data.Entities.Maintenance;
using WScore.Data.Entities.WScore;
using WScore.Entities.Identity;

namespace WScore.Configurations
{
    public class OrderStepConfiguration : IEntityTypeConfiguration<OrderStep>
    {
        public void Configure(EntityTypeBuilder<OrderStep> builder)
        {
            builder.ToTable("OrderStep", "Maintenance");
            builder.HasKey(p => p.Id)
                .HasName("PK_OrderStep");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.OrderId)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.ActionDate)
                .HasColumnType("DATE")
                .IsRequired();

            builder.HasOne(p => p.Order)
                .WithMany(p => p.OrderSteps)
                .HasForeignKey(p => p.OrderId)
                .HasConstraintName("FK_OrderStep_Order")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.Title)
                .HasColumnType("VARCHAR(150)")
                .IsRequired();

            builder.Property(p => p.OrderStepStatus)
                .HasColumnType("INT")
                .IsRequired();
        }
    }
}
