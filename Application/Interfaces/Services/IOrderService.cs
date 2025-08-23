using Application.Common;
using Application.Common.Result;
using Application.Dtos.OrderDtos;
using Application.Specificatios.Params;
using Domain.Entities;
using Domain.Entities.User;

namespace Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Result<OrderResponseDto>> CreateOrderAsync(CreateOrderDto createOrderDto, AppUser user);
        Task<Result<Pagination<OrderResponseDto>>> GetOrdersForUserAsync(string userId, PaginationParams paginationParams);
        Task<Result<OrderResponseDto>> GetOrderForUserAsync(int orderId, string userId);
        Task<Result<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethodsAsync();
        Task<Result> CancelOrderAsync(int orderId, string userId);
    }
}
