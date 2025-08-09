namespace API.Dtos.ReviewDtos
{
    public class BookReviewDto
    {
        public string ReviewText { get; set; } = null!;
        public int Rating { get; set; }
        public string UserId { get; set; } = null!;
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string AvatarPath { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }
}
