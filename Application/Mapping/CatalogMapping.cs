using Application.Dtos.CatalogDtos;
using Application.Helpers;
using Domain.Entities;

namespace Application.Mapping
{
    public static class CatalogMapping
    {
        public static CatalogBookDto MapBookToDto(Book book)
        {
            return new CatalogBookDto
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
                QuantityInStock = book.QuantityInStock,
                AuthorNames = string.Join(", ", book.Author.Select(a => a.Name))
            };
        }

        public static SingleBookDto MapBookToSingleDto(Book book)
        {
            return new SingleBookDto
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
                QuantityInStock = book.QuantityInStock,
                Genre = book.Genre,
                Author = book.Author,
                Publisher = book.Publisher
            };
        }
    }
}
