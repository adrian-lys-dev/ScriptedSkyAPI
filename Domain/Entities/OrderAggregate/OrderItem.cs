﻿using Domain.Entities;
using System.Text.Json.Serialization;

namespace Domain.Entities.OrderAggregate
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; } = null!;
        public int BookId { get; set; }
        [JsonIgnore]
        public Book Book { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
