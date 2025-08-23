using Application.Common.Result;
using Domain.Entities.Cart;

namespace Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<Result<ShoppingCart>> GetShoppingCartAsync(string key);
        Task<Result<ShoppingCart>> SetShoppingCartAsync(ShoppingCart shoppingCart);
        Task<Result> DeleteShoppingCartAsync(string key);
    }
}
