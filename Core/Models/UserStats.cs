namespace Core.Models
{
    public class UserStats
    {
        public int TotalOrders { get; set; }
        public int ActiveOrders { get; set; }
        public int TotalReviews { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
