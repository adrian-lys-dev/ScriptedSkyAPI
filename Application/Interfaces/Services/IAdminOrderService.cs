using Application.Common;
using Application.Common.Result;
using Application.Dtos.AdminOrderDtos;
using Application.Specificatios.Params;
using Domain.Entities.OrderAggregate;

namespace Application.Interfaces.Services
{
    public interface IAdminOrderService
    {
        Task<Result<Pagination<AdminOrderResponseDto>>> GetOrdersAsync(PaginationParams paginationParams);
        Task<Result> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<Result<AdminOrderResponseDto>> GetOrderByIdAsync(int orderId);
    }
}
