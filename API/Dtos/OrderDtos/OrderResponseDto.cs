namespace API.Dtos.OrderDtos
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string? Adress { get; set; }
        public DeliveryMethodDto DeliveryMethod { get; set; } = null!;
        public List<OrderItemDto> Items { get; set; } = [];
        public decimal Subtotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class DeliveryMethodDto
    {
        public string ShortName { get; set; } = string.Empty;
        public string DeliveryTime { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class OrderItemDto
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
