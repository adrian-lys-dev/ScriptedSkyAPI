using API.Dtos.CatalogDtos;
using Core.Entities;

namespace API.Mapping
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
                PictureURL = book.PictureURL,
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
                PictureURL = book.PictureURL,
                ReleaseYear = book.ReleaseYear,
                Rating = book.Rating,
                PageNumber = book.PageNumber,
                Price = book.Price,
                ISBN = book.ISBN,
                QuantityInStock = book.QuantityInStock,
                Genre = book.Genre,
                Author = book.Author,
                Publisher = book.Publisher,
                Reviews = book.Reviews.Select(r => new SBReviewDto
                {
                    Id = r.Id,
                    ReviewText = r.ReviewText,
                    Rating = r.Rating,
                    UserId = r.UserId,
                    UserName = r.AppUser.UserName!,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                }).ToList()
            };
        }

    }
}
