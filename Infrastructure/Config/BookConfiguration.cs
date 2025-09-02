using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(p => p.Id)
                .IsRequired();

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(p => p.Title)
                .IsUnique();

            builder.Property(p => p.Description)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(p => p.PictureURL)
                .IsRequired()
                .HasColumnType("text")
                .HasDefaultValue("/img/default_img.jpg");

            builder.Property(p => p.ReleaseYear)
                .IsRequired();

            builder.Property(p => p.Rating)
                .IsRequired()
                .HasColumnType("decimal(3,2)")
                .HasDefaultValue(0m);

            builder.Property(p => p.PageNumber)
                .IsRequired();

            builder.Property(p => p.QuantityInStock)
                .IsRequired();

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.ISBN)
                .IsRequired()
                .HasMaxLength(13);

            builder
                .HasMany(b => b.Genre)
                .WithMany(g => g.Book)
                .UsingEntity<Dictionary<string, object>>(
                    "BookGenre",
                    j => j
                        .HasOne<Genre>()
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Restrict), // it is impossible to delete a genre if there are books
                    j => j
                        .HasOne<Book>()
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)   // when the book is deleted, the connections are deleted
                );

            builder
                .HasMany(b => b.Author)
                .WithMany(g => g.Book)
                .UsingEntity<Dictionary<string, object>>(
                    "AuthorBook",
                    j => j
                        .HasOne<Author>()
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<Book>()
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                );

            builder
                .HasMany(b => b.Reviews)
                .WithOne(r => r.Book)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
