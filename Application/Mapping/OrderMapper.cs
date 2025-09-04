using Application.Dtos.AdminDtos.AdminOrderDtos;
using Application.Dtos.OrderDtos;
using Application.Helpers;
using Domain.Entities.OrderAggregate;

namespace Application.Mapping
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
                    PictureURL = UrlHelper.BuildImageUrl(i.Book.PictureURL),
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
                Subtotal = order.Subtotal,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt
            };
        }

        public static AdminOrderResponseDto ToAdminOrderDto(Order order)
        {
            return new AdminOrderResponseDto
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
                    PictureURL = UrlHelper.BuildImageUrl(i.Book.PictureURL),
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
                Subtotal = order.Subtotal,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                User = new UserDto
                {
                    Id = order.User.Id,
                    FirstName = order.User.FirstName,
                    LastName = order.User.LastName,
                    Email = order.User.Email!
                }
            };
        }
    }

}
