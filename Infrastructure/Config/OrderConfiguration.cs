using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.Status).HasConversion(
                o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o));

            builder.Property(o => o.ContactEmail)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(o => o.ContactName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(o => o.Adress)
                .HasMaxLength(512)
                .IsRequired(false);

            builder.Property(x => x.Subtotal)
                .HasColumnType("decimal(18,2)");

            builder.HasMany(x => x.OrderItem)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .HasForeignKey("DeliveryMethodId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.User)
                .WithMany(u => u.Order)
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Restrict).IsRequired();
        }
    }
}
