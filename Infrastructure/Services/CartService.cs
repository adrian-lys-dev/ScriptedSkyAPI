using Core.Entities.Cart;
using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class CartService(IConnectionMultiplexer redis) : ICartService
    {
        private readonly IDatabase _database = redis.GetDatabase();
        public async Task<bool> DeleteShoppingCartAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<ShoppingCart?> GetShoppingCartAsync(string key)
        {
            var data = await _database.StringGetAsync(key);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart?>(data!);
        }

        public async Task<ShoppingCart?> SetShoppingCartAsync(ShoppingCart shoppingCart)
        {
            var created = await _database.StringSetAsync(shoppingCart.Id,
                JsonSerializer.Serialize(shoppingCart), TimeSpan.FromDays(30));

            if (!created) return null;

            return await GetShoppingCartAsync(shoppingCart.Id);
        }
    }

}
