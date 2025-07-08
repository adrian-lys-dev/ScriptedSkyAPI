using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(p => p.ShortName).IsRequired().HasMaxLength(256);
            builder.Property(p => p.DeliveryTime).IsRequired().HasMaxLength(256);
            builder.Property(p => p.Description).IsRequired().HasColumnType("text");
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        }
    }
}
