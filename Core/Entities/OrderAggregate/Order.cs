using Core.Entities.Base;
using Core.Entities.User;
using System.Text.Json.Serialization;

namespace Core.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public required string ContactEmail { get; set; }
        public required string ContactName { get; set; }
        public string? Adress {  get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } = null!;
        [JsonIgnore]
        public List<OrderItem> OrderItem { get; set; } = [];
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        [JsonIgnore]
        public AppUser User { get; set; } = null!;
    }
}
