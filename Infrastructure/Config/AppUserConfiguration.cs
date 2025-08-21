using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(u => u.AvatarId)
                .HasDefaultValue(1);

            builder.HasOne(u => u.Avatar)
                   .WithMany()
                   .HasForeignKey(u => u.AvatarId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
