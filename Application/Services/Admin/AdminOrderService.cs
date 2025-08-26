using Application.Common;
using Application.Common.Result;
using Application.Dtos.OrderDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mapping;
using Application.Specificatios;
using Application.Specificatios.Params;
using Domain.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Application.Services.Admin
{
    public class AdminOrderService(IUnitOfWork unit, ILogger<AdminOrderService> logger) : IAdminOrderService
    {
        public async Task<Result<OrderResponseDto>> GetOrderByIdAsync(int orderId)
        {
            var spec = new OrderSpecification(orderId);
            var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null)
                return Result<OrderResponseDto>.Failure(new Error(ErrorType.NotFound, "Order not found"));

            return Result<OrderResponseDto>.SuccessResult(OrderMapper.ToDto(order));
        }

        public async Task<Result<Pagination<OrderResponseDto>>> GetOrdersAsync(PaginationParams paginationParams)
        {
            var spec = new OrderSpecification(paginationParams);
            var orders = await unit.Repository<Order>().ListWithSpecAsync(spec);
            var count = await unit.Repository<Order>().CountSpecAsync(spec);

            var ordersToReturn = orders.Select(OrderMapper.ToDto).ToList();
            var pagination = new Pagination<OrderResponseDto>(paginationParams.PageIndex, paginationParams.PageSize, count, ordersToReturn);

            logger.LogInformation("Fetched {Count} orders out of {Total}", ordersToReturn.Count, count);
            return Result<Pagination<OrderResponseDto>>.SuccessResult(pagination);
        }

        public async Task<Result> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await unit.Repository<Order>().GetByIdAsync(orderId);
            if (order == null)
                return Result.Failure(new Error(ErrorType.NotFound, "Order not found"));

            order.Status = status;
            unit.Repository<Order>().Update(order);

            if (await unit.Complete())
                return Result.SuccessResult();

            return Result.Failure(new Error(ErrorType.ServerError, "Failed to update order status"));
        }
    }
}
