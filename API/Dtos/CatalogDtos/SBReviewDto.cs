namespace API.Dtos.CatalogDtos
{
    public class SBReviewDto
    {
        public int Id { get; set; }
        public required string ReviewText { get; set; }
        public int Rating { get; set; }
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
