namespace API.Dtos.ReviewDtos
{
    public class ReviewDto
    {
        public string ReviewText { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
