using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WScore.Data.Entities.Car;
using WScore.Data.Entities.WScore;
using WScore.Entities.Identity;

namespace WScore.Configurations
{
    public class UserPaymentMethodConfiguration : IEntityTypeConfiguration<UserPaymentMethod>
    {
        public void Configure(EntityTypeBuilder<UserPaymentMethod> builder)
        {
            builder.ToTable("UserPaymentMethod", "WScore");

            builder.HasKey(p => p.Id)
                .HasName("PK_UserPaymentMethod");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.PaymentMethodId)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.UserId)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.CardHolderName)
                .HasColumnType("VARCHAR(150)")
                .IsRequired();

            builder.Property(p => p.CardNumber)
                .HasColumnType("VARCHAR(25)")
                .IsRequired();

            builder.Property(p => p.ExpiryDate)
                .HasColumnType("DATE")
                .IsRequired();

            builder.Property(p => p.Cvc)
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.IsPreferred)
                .HasColumnType("BIT")
                .IsRequired();

            builder.HasOne(p => p.PaymentMethod)
                .WithMany(p => p.UserPaymentMethods)
                .HasForeignKey(p => p.PaymentMethodId)
                .HasConstraintName("FK_UserPaymentMethod_PaymentMethod")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.User)
                .WithMany(p => p.UserPaymentMethods)
                .HasForeignKey(p => p.UserId)
                .HasConstraintName("FK_UserPaymentMethod_User")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
