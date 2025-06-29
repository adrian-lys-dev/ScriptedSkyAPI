using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(p => p.Id).IsRequired();

            builder.Property(p => p.Title).IsRequired().HasMaxLength(256);
            builder.HasIndex(p => p.Title).IsUnique();

            builder.Property(p => p.Description).IsRequired().HasColumnType("text");
            builder.Property(p => p.PictureURL).IsRequired().HasColumnType("text").HasDefaultValue("/img/default_img.jpg");
            builder.Property(p => p.ReleaseYear).IsRequired();
            builder.Property(p => p.Rating)
                   .IsRequired()
                   .HasColumnType("decimal(3,2)")
                   .HasDefaultValue(0m);
            builder.Property(p => p.PageNumber).IsRequired();
            builder.Property(p => p.QuantityInStock).IsRequired();
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.ISBN).IsRequired().HasMaxLength(13);
        }
    }
}
