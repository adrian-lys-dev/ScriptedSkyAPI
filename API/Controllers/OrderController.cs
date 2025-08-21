using API.Errors;
using API.Extensions;
using API.Mapping;
using API.RequestHelpers;
using Application.Dtos.OrderDtos;
using Application.Interfaces;
using Application.Specificatios;
using Application.Specificatios.Params;
using Domain.Entities;
using Domain.Entities.OrderAggregate;
using Domain.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IUnitOfWork unit, ICartService cartService, ILogger<OrderController> logger, SignInManager<AppUser> signInManager) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto createOrderDto)
        {
            logger.LogInformation("Creating order for cartId={CartId}", createOrderDto.CartId);

            var cart = await cartService.GetShoppingCartAsync(createOrderDto.CartId);
            if (cart == null)
            {
                logger.LogWarning("Cart not found for cartId={CartId}", createOrderDto.CartId);
                return BadRequest(new ApiResponse(400, "Cart not found"));
            }

            var items = new List<OrderItem>();
            decimal subtotal = 0;

            foreach (var item in cart.Items)
            {
                var book = await unit.Repository<Book>().GetByIdAsync(item.BookId);
                if (book == null)
                {
                    logger.LogWarning("Book with ID={BookId} not found in cart", item.BookId);
                    return BadRequest(new ApiResponse(400, "Problem with the order"));
                }

                items.Add(new OrderItem
                {
                    BookId = book.Id,
                    Price = book.Price,
                    Quantity = item.Quantity
                });

                subtotal += book.Price * item.Quantity;
            }

            var deliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync(createOrderDto.DeliveryMethodId);
            if (deliveryMethod == null)
            {
                logger.LogWarning("Delivery method with ID={DeliveryMethodId} not found", createOrderDto.DeliveryMethodId);
                return BadRequest(new ApiResponse(400, "Delivery method not found"));
            }

            if (deliveryMethod.Id == 3)
            {
                createOrderDto.Adress = "Pickup from the store: st. Approximate, h. 123";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(createOrderDto.Adress))
                {
                    logger.LogWarning("Address is empty for delivery method ID={DeliveryMethodId}", deliveryMethod.Id);
                    return BadRequest(new ApiResponse(400, "Address must be provided for this delivery method"));
                }
            }

            var user = await signInManager.UserManager.GetUserAsync(User);
            if (user == null)
            {
                logger.LogWarning("Unauthorized access attempt while creating order");
                return Unauthorized(new ApiResponse(401, "Unauthorized access attempt while creating order"));
            }

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
                logger.LogInformation("Order successfully created for userId={UserId}, orderId={OrderId}", user.Id, order.Id);
                return Ok(OrderMapper.ToDto(order));
            }

            logger.LogError("Failed to create order for userId={UserId}", user.Id);
            return BadRequest(new ApiResponse(400, "Failed to create order"));
        }

        [Authorize]
        [HttpGet("get-orders-for-current-user")]
        public async Task<ActionResult<IReadOnlyList<OrderResponseDto>>> GetOrdersForUser([FromQuery] PaginationParams paginationParams)
        {
            var userId = User.GetUserId();

            logger.LogInformation("User {UserId} requested their orders", userId);

            var spec = new OrderSpecification(userId, paginationParams);
            var orders = await unit.Repository<Order>().ListWithSpecAsync(spec);
            var count = await unit.Repository<Order>().CountSpecAsync(spec);
            var ordersToReturn = orders.Select(order => OrderMapper.ToDto(order)).ToList();

            var pagination = new Pagination<OrderResponseDto>(paginationParams.PageIndex, paginationParams.PageSize, count, ordersToReturn);

            logger.LogInformation("Fetched {Count} books for page {PageIndex}, user {UserId}", ordersToReturn.Count, paginationParams.PageIndex, userId);

            return Ok(pagination);
        }

        [Authorize]
        [HttpGet("get-user-order/{orderId}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrder(int orderId)
        {
            var userId = User.GetUserId();

            logger.LogInformation("User {UserId} requested order {OrderId}", userId, orderId);

            var spec = new OrderSpecification(orderId);
            var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null)
            {
                logger.LogInformation("Order {OrderId} not found for user {UserId}", orderId, userId);
                return NotFound(new ApiResponse(404, "Order not found"));
            }

            if (userId != order.User.Id)
            {
                logger.LogInformation("Order {OrderId} does not belong to user {UserId}", orderId, userId);
                return StatusCode(403, new ApiResponse(403, "You are not allowed here."));
            }

            logger.LogInformation("Order {OrderId} returned for user {UserId}", orderId, userId);
            return Ok(OrderMapper.ToDto(order));
        }

        [Authorize]
        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            logger.LogInformation("Requested list of delivery methods");
            var deliveryMethods = await unit.Repository<DeliveryMethod>().ListAllAsync();

            logger.LogInformation("Returned {Count} delivery methods", deliveryMethods.Count);
            return Ok(deliveryMethods);
        }

        [Authorize]
        [HttpPut("cancel-order/{orderId}")]
        public async Task<ActionResult> CancelOrder(int orderId)
        {
            var userId = User.GetUserId();
            logger.LogInformation("User {UserId} attempts to cancel order {OrderId}", userId, orderId);

            var spec = new OrderSpecification(orderId);
            var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null)
            {
                logger.LogWarning("Order {OrderId} not found", orderId);
                return NotFound(new ApiResponse(404, "Order not found"));
            }

            if (order.User.Id != userId)
            {
                logger.LogWarning("User {UserId} not authorized to cancel order {OrderId}", userId, orderId);
                return StatusCode(403, new ApiResponse(403, "You are not allowed to cancel this order."));
            }

            if (order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Done)
            {
                logger.LogInformation("Order {OrderId} is already in status {Status}", orderId, order.Status);
                return BadRequest(new ApiResponse(400, $"Cannot cancel an order with status '{order.Status}'."));
            }

            order.Status = OrderStatus.Cancelled;
            unit.Repository<Order>().Update(order);

            if (await unit.Complete())
            {
                logger.LogInformation("Order {OrderId} cancelled successfully by user {UserId}", orderId, userId);
                return NoContent();
            }

            logger.LogError("Failed to cancel order {OrderId} for user {UserId}", orderId, userId);
            return BadRequest(new ApiResponse(400, "Failed to cancel order"));
        }
    }
}
