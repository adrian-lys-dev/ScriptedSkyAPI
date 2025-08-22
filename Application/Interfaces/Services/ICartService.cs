using Domain.Entities.Cart;

namespace Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<ShoppingCart?> GetShoppingCartAsync(string key);
        Task<ShoppingCart?> SetShoppingCartAsync(ShoppingCart shoppingCart);
        Task<bool> DeleteShoppingCartAsync(string key);
    }
}
