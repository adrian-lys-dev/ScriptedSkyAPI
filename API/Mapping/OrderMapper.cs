using API.Dtos.OrderDtos;
using Core.Entities.OrderAggregate;

namespace API.Mapping
{
    public static class OrderMapper
    {
        public static OrderResponseDto ToDto(Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                ContactEmail = order.ContactEmail,
                ContactName = order.ContactName,
                Adress = order.Adress,
                DeliveryMethod = new DeliveryMethodDto
                {
                    ShortName = order.DeliveryMethod.ShortName,
                    DeliveryTime = order.DeliveryMethod.DeliveryTime,
                    Description = order.DeliveryMethod.Description,
                    Price = order.DeliveryMethod.Price
                },
                Items = order.OrderItem.Select(i => new OrderItemDto
                {
                    BookId = i.BookId,
                    Title = i.Book.Title,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
                Subtotal = order.Subtotal,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt
            };
        }
    }

}
