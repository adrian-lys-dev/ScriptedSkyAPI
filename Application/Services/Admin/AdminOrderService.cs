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
            var spec = new OrderSpecification(orderId);
            var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null)
                return Result.Failure(new Error(ErrorType.NotFound, "Order not found"));

            order.Status = status;

            if (status == OrderStatus.Done)
            {
                var stockResult = await DecreaseBookStockAsync(order);
                if (!stockResult.Success)
                    return stockResult;
            }

            unit.Repository<Order>().Update(order);

            if (await unit.Complete())
                return Result.SuccessResult();

            return Result.Failure(new Error(ErrorType.ServerError, "Failed to update order status"));
        }


        private async Task<Result> DecreaseBookStockAsync(Order order)
        {
            foreach (var item in order.OrderItem)
            {
                var book = await unit.Repository<Book>().GetByIdAsync(item.BookId);
                if (book == null)
                    return Result.Failure(new Error(ErrorType.NotFound, $"Book with ID {item.BookId} not found"));

                if (book.QuantityInStock < item.Quantity)
                    return Result.Failure(new Error(ErrorType.BadRequest, $"Not enough stock for book {book.Title}"));

                book.QuantityInStock -= item.Quantity;
                unit.Repository<Book>().Update(book);
            }

            return Result.SuccessResult();
        }
    }
}
