using API.Extensions;
using Application.Interfaces.Services;
using Domain.Entities.Cart;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService cartService, ILogger<CartController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetCartById(string id)
        {
            logger.LogInformation("Getting cart with ID: {CartId}", id);
            var result = await cartService.GetShoppingCartAsync(id);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            logger.LogInformation("Updating cart with ID: {CartId}", cart.Id);
            var result = await cartService.SetShoppingCartAsync(cart);
            return result.ToActionResult();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCart(string id)
        {
            logger.LogInformation("Attempting to delete cart with ID: {CartId}", id);
            var result = await cartService.DeleteShoppingCartAsync(id);
            return result.ToActionResult();
        }
    }
}
