using Application.Common;
using Application.Common.Result;
using Application.Dtos.OrderDtos;
using Application.Specificatios.Params;
using Domain.Entities.OrderAggregate;

namespace Application.Interfaces.Services
{
    public interface IAdminOrderService
    {
        Task<Result<Pagination<OrderResponseDto>>> GetOrdersAsync(PaginationParams paginationParams);
        Task<Result> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<Result<OrderResponseDto>> GetOrderByIdAsync(int orderId);
    }
}
