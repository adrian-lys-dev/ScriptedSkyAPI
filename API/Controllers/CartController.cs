using API.Errors;
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
            var cart = await cartService.GetShoppingCartAsync(id);

            logger.LogInformation("Cart with ID {CartId} retrieved successfully.", id);
            return Ok(cart ?? new ShoppingCart { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            logger.LogInformation("Updating cart with ID: {CartId}", cart.Id);
            var updatedCart = await cartService.SetShoppingCartAsync(cart);

            if (updatedCart == null)
            {
                logger.LogWarning("Failed to update cart with ID: {CartId}", cart.Id);
                return BadRequest(new ApiResponse(400, "Problem with cart"));
            }

            logger.LogInformation("Cart with ID {CartId} updated successfully.", cart.Id);
            return Ok(updatedCart);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCart(string id)
        {
            logger.LogInformation("Attempting to delete cart with ID: {CartId}", id);
            var result = await cartService.DeleteShoppingCartAsync(id);

            if (!result)
            {
                logger.LogWarning("Failed to delete cart with ID: {CartId}", id);
                return BadRequest(new ApiResponse(400, "Problem deleting cart"));
            }

            logger.LogInformation("Cart with ID {CartId} deleted successfully.", id);
            return Ok();
        }
    }
}
