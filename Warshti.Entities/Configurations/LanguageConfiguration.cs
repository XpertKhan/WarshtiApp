using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warshti.Entities.Car;
using Warshti.Entities.WScore;
using WScore.Entities.Identity;

namespace Warshti.Entities.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable("Language", "WScore");

            builder.HasKey(p => p.Id)
                .HasName("PK_Language");

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .HasColumnType("VARCHAR(150)")
                .IsRequired();

        }
    }
}
