using Microsoft.AspNetCore.Http;

namespace Application.Dtos.BookDtos
{
    public class CreateBookDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public required IFormFile Picture { get; set; }
        public int ReleaseYear { get; set; }
        public int PageNumber { get; set; }
        public decimal Price { get; set; }
        public string ISBN { get; set; } = null!;
        public int QuantityInStock { get; set; }
        public List<int> GenreIds { get; set; } = [];
        public List<int> AuthorIds { get; set; } = [];
        public int PublisherId { get; set; }
    }
}
