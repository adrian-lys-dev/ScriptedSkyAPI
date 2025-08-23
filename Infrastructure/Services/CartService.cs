using Application.Common.Result;
using Application.Interfaces.Services;
using Domain.Entities.Cart;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class CartService(IConnectionMultiplexer redis) : ICartService
    {
        private readonly IDatabase _database = redis.GetDatabase();

        public async Task<Result> DeleteShoppingCartAsync(string key)
        {
            var deleted = await _database.KeyDeleteAsync(key);
            if (!deleted)
                return Result.Failure(new Error(ErrorType.BadRequest, $"Failed to delete cart with ID {key}"));

            return Result.SuccessResult();
        }

        public async Task<Result<ShoppingCart>> GetShoppingCartAsync(string key)
        {
            var data = await _database.StringGetAsync(key);
            if (data.IsNullOrEmpty)
                return Result<ShoppingCart>.SuccessResult(new ShoppingCart { Id = key });

            try
            {
                var cart = JsonSerializer.Deserialize<ShoppingCart>(data!);
                return Result<ShoppingCart>.SuccessResult(cart!);
            }
            catch
            {
                return Result<ShoppingCart>.Failure(new Error(ErrorType.ServerError, "Failed to deserialize shopping cart"));
            }
        }

        public async Task<Result<ShoppingCart>> SetShoppingCartAsync(ShoppingCart shoppingCart)
        {
            var created = await _database.StringSetAsync(shoppingCart.Id,
                JsonSerializer.Serialize(shoppingCart), TimeSpan.FromDays(30));

            if (!created)
                return Result<ShoppingCart>.Failure(new Error(ErrorType.BadRequest, $"Failed to update cart with ID {shoppingCart.Id}"));

            var cart = await GetShoppingCartAsync(shoppingCart.Id);
            return cart;
        }
    }
}
