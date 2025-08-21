namespace Domain.Entities.Cart
{
    public class CartItem
    {
        public int BookId { get; set; }
        public required string BookName { get; set; }
        public required string AuthorName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public required string PictureURL { get; set; }
    }
}
