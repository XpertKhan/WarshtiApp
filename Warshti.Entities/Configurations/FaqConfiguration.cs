using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warshti.Entities.Car;
using Warshti.Entities.WScore;
using WScore.Entities.Identity;

namespace Warshti.Entities.Configurations
{
    public class FaqConfiguration : IEntityTypeConfiguration<Faq>
    {
        public void Configure(EntityTypeBuilder<Faq> builder)
        {
            builder.ToTable("Faq", "WScore");

            builder.HasKey(p => p.Id)
                .HasName("PK_Faq");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Question)
                .HasColumnType("VARCHAR(2000)")
                .IsRequired();

            builder.Property(p => p.Answer)
                .HasColumnType("VARCHAR(2000)")
                .IsRequired();

        }
    }
}
