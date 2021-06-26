using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WScore.Entities.Identity;

namespace WScore.Configurations
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("UserClaim", "Identity");

            builder.HasKey(uc => uc.Id);

            builder.HasOne(ur => ur.User)
                .WithMany(ur => ur.UserClaims)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        }
    }
}
