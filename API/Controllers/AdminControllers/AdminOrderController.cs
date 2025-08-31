using API.Extensions;
using Application.Dtos.AdminOrderDtos;
using Application.Dtos.OrderDtos;
using Application.Interfaces.Services;
using Application.Specificatios.Params;
using Domain.Entities.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminOrderController (IAdminOrderService adminOrderService, ILogger<AdminOrderController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AdminOrderResponseDto>>> GetOrders([FromQuery] PaginationParams paginationParams)
        {
            logger.LogInformation("Admin requested list of orders with pagination: PageIndex={PageIndex}, PageSize={PageSize}",
                paginationParams.PageIndex, paginationParams.PageSize);

            var result = await adminOrderService.GetOrdersAsync(paginationParams);
            return result.ToActionResult();
        }

        [HttpPut("update-status/{orderId}")]
        public async Task<ActionResult> UpdateOrderStatus(int orderId, [FromQuery] OrderStatus status)
        {
            logger.LogInformation("Admin requested to update status of order {OrderId} to {Status}", orderId, status);

            var result = await adminOrderService.UpdateOrderStatusAsync(orderId, status);
            return result.ToActionResult();
        }

        [HttpGet("get-order/{orderId}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrder(int orderId)
        {
            logger.LogInformation("Admin requested order {OrderId}", orderId);

            var result = await adminOrderService.GetOrderByIdAsync(orderId);
            return result.ToActionResult();
        }
    }
}
