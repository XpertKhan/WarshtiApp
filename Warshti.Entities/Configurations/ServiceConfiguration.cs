using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warshti.Entities.Car;
using Warshti.Entities.Maintenance;
using WScore.Entities.Identity;

namespace Warshti.Entities.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("Service", "Maintenance");

            builder.HasKey(p => p.Id)
                .HasName("PK_Service");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Description)
                .HasColumnType("VARCHAR(1000)")
                .IsRequired();

            builder.Property(p => p.CompanyId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.Company)
                .WithMany(p => p.CarServices)
                .HasForeignKey(p => p.CompanyId)
                .HasConstraintName("FK_Service_Company")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.ModelId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.Model)
                .WithMany(p => p.CarServices)
                .HasForeignKey(p => p.ModelId)
                .HasConstraintName("FK_Service_Model")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.ColorId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.Color)
                .WithMany(p => p.CarServices)
                .HasForeignKey(p => p.ColorId)
                .HasConstraintName("FK_Service_Color")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.TransmissionId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.Transmission)
                .WithMany(p => p.CarServices)
                .HasForeignKey(p => p.TransmissionId)
                .HasConstraintName("FK_Service_Transmission")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.PaymentMethodId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.PaymentMethod)
                .WithMany(p => p.CarServices)
                .HasForeignKey(p => p.PaymentMethodId)
                .HasConstraintName("FK_Service_PaymentMethod")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.DepartmentId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.Department)
                .WithMany(p => p.CarServices)
                .HasForeignKey(p => p.DepartmentId)
                .HasConstraintName("FK_Service_Department")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.UserId)
                .HasColumnType("INT")
                .IsRequired();

            builder.HasOne(p => p.User)
                .WithMany(p => p.Services)
                .HasForeignKey(p => p.UserId)
                .HasConstraintName("FK_Service_User")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.ServiceStatus)
                .HasColumnType("INT")
                .IsRequired()
                .HasDefaultValueSql("1");
        }
    }
}
