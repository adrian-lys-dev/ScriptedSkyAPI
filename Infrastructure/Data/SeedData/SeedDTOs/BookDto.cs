namespace Infrastructure.Data.SeedData.SeedDTOs
{
    public class BookDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureURL { get; set; } = null!;
        public int ReleaseYear { get; set; }
        public decimal Rating { get; set; }
        public int PageNumber { get; set; }
        public decimal Price { get; set; }
        public string ISBN { get; set; } = null!;
        public int QuantityInStock { get; set; }
        public int PublisherId { get; set; }
        public List<int>? GenreIds { get; set; }
        public List<int>? AuthorIds { get; set; }
    }
}
