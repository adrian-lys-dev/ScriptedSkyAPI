using API.Extensions;
using Application.Dtos.OrderDtos;
using Application.Interfaces.Services;
using Application.Specificatios.Params;
using Domain.Entities;
using Domain.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(ILogger<OrderController> logger, SignInManager<AppUser> signInManager, IOrderService orderService) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            var user = await signInManager.UserManager.GetUserAsync(User);
            var result = await orderService.CreateOrderAsync(createOrderDto, user!);
            return result.ToActionResult();
        }

        [Authorize]
        [HttpGet("get-orders-for-current-user")]
        public async Task<ActionResult<IReadOnlyList<OrderResponseDto>>> GetOrdersForUser([FromQuery] PaginationParams paginationParams)
        {
            var userId = User.GetUserId();
            logger.LogInformation("User {UserId} requested their orders", userId);

            var result = await orderService.GetOrdersForUserAsync(userId, paginationParams);

            return result.ToActionResult();
        }

        [Authorize]
        [HttpGet("get-user-order/{orderId}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrder(int orderId)
        {
            var userId = User.GetUserId();

            logger.LogInformation("User {UserId} requested order {OrderId}", userId, orderId);
            var result = await orderService.GetOrderForUserAsync(orderId, userId);

            return result.ToActionResult();
        }

        [Authorize]
        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            logger.LogInformation("Requested list of delivery methods");
            var result = await orderService.GetDeliveryMethodsAsync();

            return result.ToActionResult();
        }

        [Authorize]
        [HttpPut("cancel-order/{orderId}")]
        public async Task<ActionResult> CancelOrder(int orderId)
        {
            var userId = User.GetUserId();
            logger.LogInformation("User {UserId} attempts to cancel order {OrderId}", userId, orderId);

            var result = await orderService.CancelOrderAsync(orderId, userId);
            return result.ToActionResult();
        }
    }
}
