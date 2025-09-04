using Application.Dtos.OrderDtos;

namespace Application.Dtos.AdminDtos.AdminOrderDtos
{
    public class AdminOrderResponseDto
    {
        public int Id { get; set; }
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string? Adress { get; set; }
        public DeliveryMethodDto DeliveryMethod { get; set; } = null!;
        public List<OrderItemDto> Items { get; set; } = [];
        public UserDto User { get; set; } = null!;
        public decimal Subtotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
