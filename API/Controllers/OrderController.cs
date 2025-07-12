using API.Dtos.OrderDtos;
using API.Extensions;
using API.Mapping;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specificatios;
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
                return BadRequest("Cart not found");
            }

            var items = new List<OrderItem>();
            decimal subtotal = 0;

            foreach (var item in cart.Items)
            {
                var book = await unit.Repository<Book>().GetByIdAsync(item.BookId);
                if (book == null)
                {
                    logger.LogWarning("Book with ID={BookId} not found in cart", item.BookId);
                    return BadRequest("Problem with the order");
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
                return BadRequest("Delivery method not found");
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
                    return BadRequest("Address must be provided for this delivery method");
                }
            }

            var user = await signInManager.UserManager.GetUserAsync(User);
            if (user == null)
            {
                logger.LogWarning("Unauthorized access attempt while creating order");
                return Unauthorized();
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
            return BadRequest("Failed to create order");
        }

        [Authorize]
        [HttpGet("get-orders-for-current-user")]
        public async Task<ActionResult<IReadOnlyList<OrderResponseDto>>> GetOrdersForUser()
        {
            var userId = User.GetUserId();

            logger.LogInformation("User {UserId} requested their orders", userId);

            var spec = new OrderSpecification(userId);

            var orders = await unit.Repository<Order>().ListWithSpecAsync(spec);

            var ordersToReturn = orders.Select(order => OrderMapper.ToDto(order)).ToList();

            logger.LogInformation("Returned {Count} orders for user {UserId}", ordersToReturn.Count, userId);

            return Ok(ordersToReturn);
        }

        [Authorize]
        [HttpGet("get-order/{orderId}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrder(int orderId)
        {
            var userId = User.GetUserId();

            logger.LogInformation("User {UserId} requested order {OrderId}", userId, orderId);

            var spec = new OrderSpecification(orderId);
            var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null)
            {
                logger.LogInformation("Order {OrderId} not found for user {UserId}", orderId, userId);
                return NotFound("Order not found.");
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
    }
}
