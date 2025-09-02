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

    }
}
