namespace Core.Entities.OrderAggregate
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
