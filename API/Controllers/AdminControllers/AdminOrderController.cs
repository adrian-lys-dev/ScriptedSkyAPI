using API.Dtos.OrderDtos;
using API.Mapping;
using API.RequestHelpers;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specificatios;
using Core.Specificatios.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Re_ABP_Backend.Errors;

namespace API.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminOrderController (IUnitOfWork unit, ILogger<OrderController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderResponseDto>>> GetOrdersForUser([FromQuery] PaginationParams paginationParams)
        {
            logger.LogInformation("Admin requested list of orders with pagination: PageIndex={PageIndex}, PageSize={PageSize}",
                paginationParams.PageIndex, paginationParams.PageSize);

            var spec = new OrderSpecification(paginationParams);
            var orders = await unit.Repository<Order>().ListWithSpecAsync(spec);
            var count = await unit.Repository<Order>().CountSpecAsync(spec);

            logger.LogInformation("Returned {Count} orders out of {Total} total", orders.Count, count);

            var ordersToReturn = orders.Select(order => OrderMapper.ToDto(order)).ToList();
            var pagination = new Pagination<OrderResponseDto>(paginationParams.PageIndex, paginationParams.PageSize, count, ordersToReturn);

            return Ok(pagination);
        }

        [HttpPut("update-status/{orderId}")]
        public async Task<ActionResult> UpdateOrderStatus(int orderId, [FromQuery] OrderStatus status)
        {
            logger.LogInformation("Admin requested to update status of order {OrderId} to {Status}", orderId, status);

            var order = await unit.Repository<Order>().GetByIdAsync(orderId);
            if (order == null)
            {
                logger.LogWarning("Order {OrderId} not found", orderId);
                return NotFound(new ApiResponse(404, "Order not found"));
            }

            order.Status = status;
            unit.Repository<Order>().Update(order);

            if (await unit.Complete())
            {
                logger.LogInformation("Order {OrderId} status updated to {Status} by admin", orderId, status);
                return NoContent();
            }

            logger.LogError("Failed to update status for order {OrderId}", orderId);
            return BadRequest(new ApiResponse(400, "Failed to update order status"));
        }
    }
}
