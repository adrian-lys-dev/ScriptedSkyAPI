using Application.Common;
using Application.Common.Result;
using Application.Dtos.OrderDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mapping;
using Application.Specificatios;
using Application.Specificatios.Params;
using Domain.Entities;
using Domain.Entities.OrderAggregate;
using Domain.Entities.User;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class OrderService(IUnitOfWork unit, ICartService cartService, ILogger<OrderService> logger) : IOrderService
    {
        public async Task<Result<OrderResponseDto>> CreateOrderAsync(CreateOrderDto createOrderDto, AppUser user)
        {
            logger.LogInformation("Creating order for cartId={CartId}, userId={UserId}", createOrderDto.CartId, user.Id);

            if (user == null)
                return Result<OrderResponseDto>.Failure(new Error(ErrorType.Unauthorized, "Unauthorized"));

            var cart = await cartService.GetShoppingCartAsync(createOrderDto.CartId);
            if (cart == null)
                return Result<OrderResponseDto>.Failure(new Error(ErrorType.BadRequest, "Cart not found"));

            var items = new List<OrderItem>();
            decimal subtotal = 0;

            foreach (var item in cart.Items)
            {
                var book = await unit.Repository<Book>().GetByIdAsync(item.BookId);
                if (book == null)
                    return Result<OrderResponseDto>.Failure(new Error(ErrorType.BadRequest, $"Book {item.BookId} not found"));

                items.Add(new OrderItem { BookId = book.Id, Price = book.Price, Quantity = item.Quantity });
                subtotal += book.Price * item.Quantity;
            }

            var deliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync(createOrderDto.DeliveryMethodId);
            if (deliveryMethod == null)
                return Result<OrderResponseDto>.Failure(new Error(ErrorType.BadRequest, "Delivery method not found"));

            if (deliveryMethod.Id == 3)
                createOrderDto.Adress = "Pickup from the store: st. Approximate, h. 123";
            else if (string.IsNullOrWhiteSpace(createOrderDto.Adress))
                return Result<OrderResponseDto>.Failure(new Error(ErrorType.BadRequest, "Address must be provided"));

            var order = new Order
            {
                ContactEmail = createOrderDto.ContactEmail,
                ContactName = createOrderDto.ContactName,
                Adress = createOrderDto.Adress,
                DeliveryMethod = deliveryMethod,
                OrderItem = items,
                Subtotal = subtotal + deliveryMethod.Price,
                Status = OrderStatus.Pending,
                User = user
            };

            unit.Repository<Order>().Add(order);

            if (await unit.Complete())
            {
                await cartService.DeleteShoppingCartAsync(createOrderDto.CartId);
                return Result<OrderResponseDto>.SuccessResult(OrderMapper.ToDto(order));
            }

            return Result<OrderResponseDto>.Failure(new Error(ErrorType.ServerError, "Failed to create order"));
        }

        public async Task<Result<Pagination<OrderResponseDto>>> GetOrdersForUserAsync(string userId, PaginationParams paginationParams)
        {
            var spec = new OrderSpecification(userId, paginationParams);
            var orders = await unit.Repository<Order>().ListWithSpecAsync(spec);
            var count = await unit.Repository<Order>().CountSpecAsync(spec);
            var dtos = orders.Select(OrderMapper.ToDto).ToList();

            var pagination = new Pagination<OrderResponseDto>(paginationParams.PageIndex, paginationParams.PageSize, count, dtos);
            return Result<Pagination<OrderResponseDto>>.SuccessResult(pagination);
        }

        public async Task<Result<OrderResponseDto>> GetOrderForUserAsync(int orderId, string userId)
        {
            var spec = new OrderSpecification(orderId);
            var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null)
                return Result<OrderResponseDto>.Failure(new Error(ErrorType.NotFound, "Order not found"));

            if (order.User.Id != userId)
                return Result<OrderResponseDto>.Failure(new Error(ErrorType.Forbidden, "Not allowed to view this order"));

            return Result<OrderResponseDto>.SuccessResult(OrderMapper.ToDto(order));
        }

        public async Task<Result<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await unit.Repository<DeliveryMethod>().ListAllAsync();
            logger.LogInformation("Returned {Count} delivery methods", deliveryMethods.Count);
            return Result<IReadOnlyList<DeliveryMethod>>.SuccessResult(deliveryMethods);
        }

        public async Task<Result> CancelOrderAsync(int orderId, string userId)
        {
            var spec = new OrderSpecification(orderId);
            var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null)
                return Result.Failure(new Error(ErrorType.NotFound, "Order not found"));

            if (order.User.Id != userId)
                return Result.Failure(new Error(ErrorType.Forbidden, "Not allowed to cancel this order"));

            if (order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Done)
                return Result.Failure(new Error(ErrorType.BadRequest, $"Cannot cancel an order with status '{order.Status}'"));

            order.Status = OrderStatus.Cancelled;
            unit.Repository<Order>().Update(order);

            if (await unit.Complete())
                return Result.SuccessResult();

            return Result.Failure(new Error(ErrorType.ServerError, "Failed to cancel order"));
        }
    }
}
