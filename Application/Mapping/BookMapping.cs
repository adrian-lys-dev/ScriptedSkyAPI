using Application.Dtos.BookDtos;
using Application.Dtos.FilteringDtos;
using Application.Helpers;
using Domain.Entities;

namespace Application.Mapping
{
    public static class BookMapping
    {
        public static BookDto ToDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                PictureURL = UrlHelper.BuildImageUrl(book.PictureURL),
                Rating = book.Rating,
                Price = book.Price,
                QuantityInStock = book.QuantityInStock,
                AuthorNames = string.Join(", ", book.Author.Select(a => a.Name)),
                GenreNames = string.Join(", ", book.Genre.Select(a => a.Name)),
                PubliherName = book.Publisher.Name,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt
            };
        }

        public static BookDetailsDto ToDetailsDto(Book book)
        {
            return new BookDetailsDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                PictureURL = UrlHelper.BuildImageUrl(book.PictureURL),
                ReleaseYear = book.ReleaseYear,
                Rating = book.Rating,
                PageNumber = book.PageNumber,
                Price = book.Price,
                ISBN = book.ISBN,
                QuantityInStock= book.QuantityInStock,
                Genre = book.Genre.Select(i => new FilteringDto
                {
                    Id = i.Id,
                    Name = i.Name,
                }).ToList(),
                Author = book.Author.Select(i => new FilteringDto 
                {
                    Id = i.Id,
                    Name = i.Name,
                }).ToList(),
                Publisher = new FilteringDto
                {
                    Id = book.Publisher.Id,
                    Name = book.Publisher.Name,
                },
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt
            };
        }

        public static Book ToEntity(CreateBookDto dto, Author[] authors, Genre[] genres, Publisher publisher, string pictureUrl)
        {
            return new Book
            {
                Title = dto.Title,
                Description = dto.Description,
                PictureURL = pictureUrl,
                ReleaseYear = dto.ReleaseYear,
                PageNumber = dto.PageNumber,
                Price = dto.Price,
                ISBN = dto.ISBN,
                QuantityInStock = dto.QuantityInStock,
                Publisher = publisher,
                Author = authors.Where(a => a != null).ToList(),
                Genre = genres.Where(g => g != null).ToList()
            };
        }

        public static void UpdateEntity(Book book, CreateBookDto dto, Author[] authors, Genre[] genres, Publisher publisher, string? pictureUrl = null)
        {
            book.Title = dto.Title;
            book.Description = dto.Description;
            book.ReleaseYear = dto.ReleaseYear;
            book.PageNumber = dto.PageNumber;
            book.Price = dto.Price;
            book.QuantityInStock = dto.QuantityInStock;
            book.Publisher = publisher;

            var authorsToRemove = book.Author.Where(a => !authors.Contains(a)).ToList();
            foreach (var a in authorsToRemove)
                book.Author.Remove(a);

            var authorsToAdd = authors.Where(a => !book.Author.Contains(a));
            foreach (var a in authorsToAdd)
                book.Author.Add(a);

            var genresToRemove = book.Genre.Where(g => !genres.Contains(g)).ToList();
            foreach (var g in genresToRemove)
                book.Genre.Remove(g);

            var genresToAdd = genres.Where(g => !book.Genre.Contains(g));
            foreach (var g in genresToAdd)
                book.Genre.Add(g);

            if (!string.IsNullOrEmpty(pictureUrl))
                book.PictureURL = pictureUrl;
        }

    }
}
