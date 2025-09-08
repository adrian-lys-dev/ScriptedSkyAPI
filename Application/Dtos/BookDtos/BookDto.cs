namespace Application.Dtos.BookDtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string PictureURL { get; set; }
        public decimal Rating { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public required string AuthorNames { get; set; }
        public required string GenreNames { get; set; }
        public required string PublisherName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
