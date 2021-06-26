﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WScore.Entities.Identity;

namespace Warshti.Entities.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User", "Identity");

            builder.Property(p => p.UserName)
                .HasColumnType("VARCHAR(25)")
                .IsRequired();

            builder.Property(p => p.Email)
                .HasColumnType("VARCHAR(350)")
                .IsRequired();

            builder.Property(p => p.PhoneNumber)
                .HasColumnType("VARCHAR(25)");

            builder.Property(p => p.Name)
                .HasColumnType("VARCHAR(250)")
                .IsRequired();

            builder.Property(p => p.VerificationToken)
                .HasColumnType("VARCHAR(500)")
                .IsRequired();

            builder.Property(p => p.IsActive)
                .HasColumnType("BIT")
                .IsRequired();

            builder.Property(p => p.Created)
                .HasColumnType("DATETIME")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

        }
    }
}
