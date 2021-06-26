using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WScore.Entities.Identity;

namespace Warshti.Entities.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken", "Identity");

            builder.HasKey(rc => rc.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.CreationDate)
                .HasColumnType("DATETIME")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.ExpiryDate)
                .HasColumnType("DATETIME")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.Invalidated)
                .HasColumnType("BIT")
                .IsRequired();

            builder.Property(p => p.Used)
               .HasColumnType("BIT")
               .IsRequired();

            builder.Property(p => p.UserId)
               .HasColumnType("INT")
               .IsRequired();

            builder.Property(p => p.JwtId)
                .HasColumnType("VARCHAR(150)")
                .IsRequired();

            builder.HasOne(p => p.User)
               .WithMany(p => p.RefreshTokens)
               .HasForeignKey(p => p.UserId)
               .HasConstraintName("FK_RefreshToken_User")
               .IsRequired()
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
