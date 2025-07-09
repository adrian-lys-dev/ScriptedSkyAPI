using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.Property(p => p.Id).IsRequired();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(p => p.Name)
                .IsUnique();
        }
    }
}
